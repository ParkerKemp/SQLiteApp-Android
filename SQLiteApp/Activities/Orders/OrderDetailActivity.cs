using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using Newtonsoft.Json;
using SQLiteApp.Activities;

namespace SQLiteApp
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	class OrderDetailActivity : SQLiteAppActivity
	{
		Order _order;
		string _storeName;
		Database _database;
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.OrderDetails);

			_order = JsonConvert.DeserializeObject<Order>(Intent.GetStringExtra("Order"));
			_storeName = Intent.GetStringExtra("StoreName");

			ActionBar.Title = "Order from " + _storeName;
			ActionBar.SetHomeButtonEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetDisplayShowHomeEnabled(false);
		}

		protected override void OnResume()
		{
			base.OnResume();

			PopulateView();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.EditDeleteMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					break;
				case Resource.Id.edit:
					EditOrder();
					break;
				case Resource.Id.delete:
					ConfirmDelete();
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (resultCode == Result.Ok)
			{
				_order = JsonConvert.DeserializeObject<Order>(data.GetStringExtra("Order"));
			}
		}

		private void EditOrder()
		{
			Intent editOrderIntent = new Intent(this, typeof(EditOrderActivity));
			editOrderIntent.PutExtra("Order", JsonConvert.SerializeObject(_order));
			StartActivityForResult(editOrderIntent, 1);
		}

		private void GetDatabase()
		{
			var dir = Android.OS.Environment.ExternalStorageDirectory.ToString();
			string filename = "Stores.sqlite";
			var dbName = dir + "/" + filename;

			_database = new Database(dbName);
		}

		private void ConfirmDelete()
		{
			AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle("Delete this order?");
			alert.SetPositiveButton("Yes", (s, a) =>
			{
				DeleteOrder();
			});

			alert.SetNegativeButton("No", (s, a) =>
			{
			});

			alert.Show();
		}

		private void DeleteOrder()
		{
			GetDatabase();
			_database.DeleteOrder(_order.OrderID);

			Toast.MakeText(ApplicationContext, "Order deleted.", ToastLength.Short).Show();
			ReturnEdited(true);
		}
		
		protected override void PopulateView()
		{
			FindViewById<TextView>(Resource.Id.orderID).Text = _order.OrderID;
			FindViewById<TextView>(Resource.Id.date).Text = _order.Date;
			FindViewById<TextView>(Resource.Id.totalItems).Text = _order.TotalItems.ToString();
			FindViewById<TextView>(Resource.Id.totalCost).Text = "$" + string.Format("{0:0.00}", _order.TotalCost);
			FindViewById<TextView>(Resource.Id.contactName).Text = _order.ContactName;
			FindViewById<TextView>(Resource.Id.rushOrder).Text = _order.RushOrder ? "Yes" : "No";
		}
	}
}