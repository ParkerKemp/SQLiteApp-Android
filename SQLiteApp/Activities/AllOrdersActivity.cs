using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SQLiteApp
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, Theme = "@android:style/Theme.NoTitleBar")]
	class AllOrdersActivity : Activity
	{
		Database _database;
		ListView _orderList;
		string _storeName;
		string _storeID;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.AllOrders);

			GetExtras();

			GetUIComponents();

			GetDatabase();

			PopulateView();
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
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
			StartActivity(orderIntent);
		}

		private void PopulateView()
		{
			Order[] orders = _database.GetOrdersForStore(_storeID);

			OrderSummaryAdapter adapter = new OrderSummaryAdapter(this, orders);
			_orderList.Adapter = adapter;
			FindViewById<TextView>(Resource.Id.storeName).Text = "Orders from " + _storeName;
		}
	}
}