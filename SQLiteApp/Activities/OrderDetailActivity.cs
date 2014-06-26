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

namespace SQLiteApp
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	class OrderDetailActivity : Activity
	{
		Order _order;
		string _storeName;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.OrderDetails);

			_order = JsonConvert.DeserializeObject<Order>(Intent.GetStringExtra("Order"));
			_storeName = Intent.GetStringExtra("StoreName");

			PopulateView();

			ActionBar.Title = "Order from " + _storeName;
			ActionBar.SetHomeButtonEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetDisplayShowHomeEnabled(false);
		}
		
		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
		}

		private void PopulateView()
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