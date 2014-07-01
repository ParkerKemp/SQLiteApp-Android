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
	class NewStoreActivity : StoreEntryActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ActionBar.Title = "Add new store";

			Button addStore = FindViewById<Button>(Resource.Id.addStore);
			addStore.Click += (s, e) =>
				{
					Store store;
					if ((store = AddStore()) != null)
					{
						Toast.MakeText(ApplicationContext, "Store added!", ToastLength.Short).Show();
						Intent returnIntent = new Intent();
						returnIntent.PutExtra("Store", JsonConvert.SerializeObject(store));
						SetResult(Result.Ok, returnIntent);
						Finish();
					}
					else
						Toast.MakeText(ApplicationContext, "Invalid input", ToastLength.Short).Show();
				};
		}

		private Store AddStore()
		{
			if (!ValidateInputs())
				return null;

			int zip = int.Parse(FindViewById<EditText>(Resource.Id.zip).Text);
			int territoryNum = int.Parse(FindViewById<EditText>(Resource.Id.territoryNum).Text);
			
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

			Store store = new Store(storeID, storeName, storeNum, sequenceNum, managerName, 
				phoneNum, address1, address2, city, state, zip, territoryNum);

			_database.InsertStore(store);
			
			return store;
		}
	}
}