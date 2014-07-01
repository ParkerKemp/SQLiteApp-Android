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
	class OrderEntryActivity : SQLiteAppActivity
	{
		protected Database _database;
		protected string _storeID;
		protected string _storeName;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.NewOrder);

			_storeID = Intent.GetStringExtra("StoreID");
			_storeName = Intent.GetStringExtra("StoreName");

			ActionBar.SetDisplayShowHomeEnabled(false);
			ActionBar.SetHomeButtonEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);

			GetDatabase();
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

		private void GetDatabase()
		{
			var dir = Android.OS.Environment.ExternalStorageDirectory.ToString();
			string filename = "Stores.sqlite";
			var dbName = dir + "/" + filename;

			_database = new Database(dbName);
		}

		protected bool ValidateInputs()
		{
			bool ret = true;

			ret &= Validate(Resource.Id.totalCost, Order.ValidCost);
			ret &= Validate(Resource.Id.totalItems, Order.ValidTotalItems);
			ret &= Validate(Resource.Id.rushOrder, Order.ValidRushOrder);
			ret &= Validate(Resource.Id.date, Order.ValidDate);
			ret &= Validate(Resource.Id.contactName, Order.ValidContactName);

			return ret;
		}

		private bool Validate(int id, Func<string, bool> validateFunc)
		{
			if (!validateFunc(FindViewById<EditText>(id).Text))
			{
				Shade(id, "#FFDDDD");
				return false;
			}

			Shade(id, "#FFFFFF");
			return true;
		}

		protected void Shade(int id, string color)
		{
			FindViewById<EditText>(id).SetBackgroundColor(Android.Graphics.Color.ParseColor(color));
		}
	}
}
