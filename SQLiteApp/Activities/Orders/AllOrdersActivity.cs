using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using SQLiteApp.Activities;
using System;
using System.Collections.Generic;

namespace SQLiteApp
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	class AllOrdersActivity : SQLiteAppActivity
	{
		private Database _database;
		private ListView _orderList;
		private string _storeName;
		private string _storeID;
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.AllOrders);

			GetExtras();

			GetUIComponents();

			GetDatabase();

			ActionBar.Title = "Orders from " + _storeName;
			ActionBar.SetHomeButtonEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetDisplayShowHomeEnabled(false);

			PopulateView();
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					break;
				case Resource.Id.addNew:
					AddNewOrder();
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
		}

		private void AddNewOrder()
		{
			Intent addStoreIntent = new Intent(this, typeof(NewOrderActivity));
			addStoreIntent.PutExtra("StoreID", _storeID);
			addStoreIntent.PutExtra("StoreName", _storeName);
			StartActivityForResult(addStoreIntent, 2);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.PlusMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		private void GetExtras()
		{
			_storeID = Intent.GetStringExtra("StoreID");
			_storeName = Intent.GetStringExtra("StoreName");
		}

		private void GetUIComponents()
		{
			_orderList = (ListView)FindViewById(Resource.Id.orderList);
			
			_orderList.ItemClick += (s, e) =>
			{
				var adapter = (OrderSummaryAdapter)_orderList.Adapter;
				Order order = adapter[e.Position];
				ViewOrderDetails(order);
			};
		}

		private void GetDatabase()
		{
			var dir = Android.OS.Environment.ExternalStorageDirectory.ToString();
			string filename = "Stores.sqlite";
			var dbName = dir + "/" + filename;

			_database = new Database(dbName);
		}

		private void ViewOrderDetails(Order order)
		{
			Intent orderIntent = new Intent(this, typeof(OrderDetailActivity));
			orderIntent.PutExtra("Order", JsonConvert.SerializeObject(order));
			orderIntent.PutExtra("StoreName", _storeName);
			StartActivityForResult(orderIntent, 1);
		}

		protected override void PopulateView()
		{
			Order[] orders = _database.GetOrdersForStore(_storeID);

			OrderSummaryAdapter adapter = new OrderSummaryAdapter(this, orders);
			_orderList.Adapter = adapter;
		}
	}
}