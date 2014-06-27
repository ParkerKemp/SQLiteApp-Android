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

namespace SQLiteApp.Activities
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	class NewStoreActivity : Activity
	{
		private Database _database;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.NewStore);

			GetDatabase();

			ActionBar.Title = "Add new store";

			ActionBar.SetDisplayShowHomeEnabled(false);
			ActionBar.SetHomeButtonEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);

			Button addStore = FindViewById<Button>(Resource.Id.addStore);
			addStore.Click += (s, e) =>
				{
					if (AddStore())
						Console.WriteLine("Added store");
					else
						Console.WriteLine("Didn't add store.");
				};
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

		private bool AddStore()
		{
			if (!ValidateInputs())
			{
				Toast.MakeText(ApplicationContext, "Invalid input", ToastLength.Short).Show();
				return false;
			}

			int zip, territoryNum;

			try
			{
				zip = int.Parse(FindViewById<EditText>(Resource.Id.zip).Text);
			}
			catch (FormatException)
			{
				zip = -1;
			}
			try
			{
				territoryNum = int.Parse(FindViewById<EditText>(Resource.Id.territoryNum).Text);
			}
			catch (FormatException)
			{
				territoryNum = -1;
			}

			string storeName = FindViewById<EditText>(Resource.Id.storeName).Text;
			string storeID = FindViewById<EditText>(Resource.Id.storeID).Text;
			string storeNum = FindViewById<EditText>(Resource.Id.storeNum).Text;
			string sequenceNum = FindViewById<EditText>(Resource.Id.sequenceNum).Text;
			string managerName = FindViewById<EditText>(Resource.Id.managerName).Text;
			string address1 = FindViewById<EditText>(Resource.Id.address1).Text;
			string address2 = FindViewById<EditText>(Resource.Id.address2).Text;
			string city = FindViewById<EditText>(Resource.Id.city).Text;
			string state = FindViewById<EditText>(Resource.Id.state).Text;
			string phoneNum = FindViewById<EditText>(Resource.Id.phoneNum).Text;

			Store store = new Store(storeID, storeName, storeNum, sequenceNum, managerName, phoneNum, address1, address2, city, state, zip, territoryNum);

			_database.InsertStore(store);
			Toast.MakeText(ApplicationContext, "Store added!", ToastLength.Short).Show();
			OnBackPressed();
			return true;
		}

		private bool ValidateInputs()
		{
			bool ret = true;

			ret &= Validate(Resource.Id.storeID, "^[a-zA-Z0-9]{1,10}$");
			ret &= Validate(Resource.Id.storeName, "^.{1,50}$");
			ret &= Validate(Resource.Id.territoryNum, "^[0-9]{1,5}$");
			ret &= Validate(Resource.Id.sequenceNum, "^[0-9]{0,4}$");
			ret &= Validate(Resource.Id.address1, "^.{1,40}$");
			ret &= Validate(Resource.Id.address2, "^.{0,40}$");
			ret &= Validate(Resource.Id.city, "^.{1,20}$");
			ret &= Validate(Resource.Id.state, "^[A-Za-z]{2,2}$");
			ret &= Validate(Resource.Id.zip, "^[0-9]{5,9}$");
			ret &= Validate(Resource.Id.managerName, "^.{1,50}$");
			ret &= Validate(Resource.Id.phoneNum, "^[0-9 -]{10,14}$");

			return ret;
		}

		private bool Validate(int id, string pattern)
		{
			if (Regex.IsMatch(FindViewById<EditText>(id).Text, pattern))
			{
				Shade(id, "#FFFFFF");
				return true;
			}

			Shade(id, "#FFDDDD");
			return false;
		}

		private void Shade(int id, string color)
		{
			FindViewById<EditText>(id).SetBackgroundColor(Android.Graphics.Color.ParseColor(color));
		}

		private bool ValidInt(int id)
		{
			try
			{
				int.Parse(FindViewById<EditText>(id).Text);
			}
			catch (Exception)
			{
				return false;
			}
			return false;
		}
	}
}