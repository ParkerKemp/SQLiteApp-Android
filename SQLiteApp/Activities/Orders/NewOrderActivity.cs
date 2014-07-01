using Android.App;
using Android.Content;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteApp.Activities
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	class NewOrderActivity : OrderEntryActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			_storeID = Intent.GetStringExtra("StoreID");
			_storeName = Intent.GetStringExtra("StoreName");

			ActionBar.Title = "New order (" + _storeName + ")";
			
			FindViewById<Button>(Resource.Id.addOrderButton).Click += (s, e) =>
				{
					Order order;
					if ((order = AddOrder()) != null)
					{
						Toast.MakeText(ApplicationContext, "Order added!", ToastLength.Short).Show();
						ReturnEdited(true);
					}
					else
						Toast.MakeText(ApplicationContext, "Invalid input", ToastLength.Short).Show();
				};
		}

		private Order AddOrder()
		{
			if (!ValidateInputs())
				return null;

			float totalCost = float.Parse(FindViewById<EditText>(Resource.Id.totalCost).Text);
			int totalItems = int.Parse(FindViewById<EditText>(Resource.Id.totalItems).Text);
			bool rushOrder = FindViewById<EditText>(Resource.Id.rushOrder).Text.Equals("Y", StringComparison.InvariantCultureIgnoreCase);
			string date = FindViewById<EditText>(Resource.Id.date).Text;
			string contactName = FindViewById<EditText>(Resource.Id.contactName).Text;

			Order order = new Order(_storeID, Order.RandomID(_database), date, totalItems, totalCost, contactName, rushOrder);

			_database.InsertOrder(order);

			return order;
		}
	}
}
