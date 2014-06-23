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
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, Theme = "@android:style/Theme.NoTitleBar")]
	class OrderDetailActivity : Activity
	{
		Order _order;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.OrderDetails);

			_order = JsonConvert.DeserializeObject<Order>(Intent.GetStringExtra("Order"));

			PopulateView();
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
		}

		private void PopulateView()
		{
			FindViewById<TextView>(Resource.Id.storeName).Text = "Order at " + _order.StoreName;

			FindViewById<TextView>(Resource.Id.orderID).Text = _order.OrderID;
			FindViewById<TextView>(Resource.Id.date).Text = _order.Date;
			FindViewById<TextView>(Resource.Id.totalItems).Text = _order.TotalItems.ToString();
			FindViewById<TextView>(Resource.Id.totalCost).Text = "$" + string.Format("{0:0.00}", _order.TotalCost);
			FindViewById<TextView>(Resource.Id.contactName).Text = _order.ContactName;
			FindViewById<TextView>(Resource.Id.rushOrder).Text = _order.RushOrder ? "Yes" : "No";
		}
	}
}