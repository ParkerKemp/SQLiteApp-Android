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
	class EditOrderActivity : OrderEntryActivity
	{
		private Order _order;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			_order = JsonConvert.DeserializeObject<Order>(Intent.GetStringExtra("Order"));

			ActionBar.Title = "Editing order";

			FindViewById<Button>(Resource.Id.addOrderButton).Text = "Update Order";
			
			FindViewById<Button>(Resource.Id.addOrderButton).Click += (s, e) =>
				{
					Order order;
					if ((order = UpdateOrder()) != null)
					{
						Toast.MakeText(ApplicationContext, "Order updated!", ToastLength.Short).Show();
						Intent returnIntent = new Intent();
						returnIntent.PutExtra("Order", JsonConvert.SerializeObject(order));
						ReturnEdited(returnIntent, true);
					}
					else
						Toast.MakeText(ApplicationContext, "Invalid input", ToastLength.Short).Show();
				};

			PopulateView();
		}

		protected override void PopulateView()
		{
			FindViewById<TextView>(Resource.Id.totalCost).Text = string.Format("{0:0.00}", _order.TotalCost);
			FindViewById<EditText>(Resource.Id.totalItems).Text = _order.TotalItems.ToString();
			FindViewById<EditText>(Resource.Id.rushOrder).Text = _order.RushOrder ? "Y" : "N";
			FindViewById<EditText>(Resource.Id.date).Text = _order.Date;
			FindViewById<EditText>(Resource.Id.contactName).Text = _order.ContactName;
		}

		private Order UpdateOrder()
		{
			if (!ValidateInputs())
				return null;

			float totalCost = float.Parse(FindViewById<EditText>(Resource.Id.totalCost).Text);
			int totalItems = int.Parse(FindViewById<EditText>(Resource.Id.totalItems).Text);
			bool rushOrder = FindViewById<EditText>(Resource.Id.rushOrder).Text.Equals("Y", StringComparison.InvariantCultureIgnoreCase);
			string date = FindViewById<EditText>(Resource.Id.date).Text;
			string contactName = FindViewById<EditText>(Resource.Id.contactName).Text;

			Order order = new Order(_storeID, _order.OrderID, date, totalItems, totalCost, contactName, rushOrder);

			_database.UpdateOrder(order);

			return order;
		}
	}
}
