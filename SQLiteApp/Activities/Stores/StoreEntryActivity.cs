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
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SQLiteApp.Activities
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	class StoreEntryActivity : SQLiteAppActivity
	{
		protected Database _database;
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.NewStore);

			GetDatabase();

			ActionBar.SetDisplayShowHomeEnabled(false);
			ActionBar.SetHomeButtonEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
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
			
			ret &= Validate(Resource.Id.storeID, Store.ValidStoreID);
			ret &= Validate(Resource.Id.storeName, Store.ValidStoreName);
			ret &= Validate(Resource.Id.storeNum, Store.ValidStoreNum);
			ret &= Validate(Resource.Id.territoryNum, Store.ValidTerritoryNum);
			ret &= Validate(Resource.Id.sequenceNum, Store.ValidSequenceNum);
			ret &= Validate(Resource.Id.address1, Store.ValidAddress1);
			ret &= Validate(Resource.Id.address2, Store.ValidAddress2);
			ret &= Validate(Resource.Id.city, Store.ValidCity);
			ret &= Validate(Resource.Id.state, Store.ValidState);
			ret &= Validate(Resource.Id.zip, Store.ValidZip);
			ret &= Validate(Resource.Id.managerName, Store.ValidManagerName);
			ret &= Validate(Resource.Id.phoneNum, Store.ValidPhoneNum);

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