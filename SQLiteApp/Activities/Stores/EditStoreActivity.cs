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
	class EditStoreActivity : StoreEntryActivity
	{
		private Store _store;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			_store = JsonConvert.DeserializeObject<Store>(Intent.GetStringExtra("Store"));

			ActionBar.Title = "Edit store";

			FindViewById<EditText>(Resource.Id.storeID).KeyListener = null;
			Shade(Resource.Id.storeID, "#888888");

			Button updateStore = FindViewById<Button>(Resource.Id.addStore);
			updateStore.Text = "Update Store";
			updateStore.Click += (s, e) =>
				{
					Store store;
					if ((store = UpdateStore()) != null)
					{
						Toast.MakeText(ApplicationContext, "Store updated!", ToastLength.Short).Show();
						Intent returnIntent = new Intent();
						returnIntent.PutExtra("Store", JsonConvert.SerializeObject(store));
						ReturnEdited(returnIntent, true);
					}
					else
						Toast.MakeText(ApplicationContext, "Invalid input", ToastLength.Short).Show();
				};

			PopulateView();
		}

		protected override void PopulateView()
		{
			FindViewById<EditText>(Resource.Id.storeName).Text = _store.StoreName;
			FindViewById<EditText>(Resource.Id.storeID).Text = _store.StoreID;
			FindViewById<EditText>(Resource.Id.storeNum).Text = _store.StoreNum;
			FindViewById<EditText>(Resource.Id.sequenceNum).Text = _store.SequenceNum;
			FindViewById<EditText>(Resource.Id.managerName).Text = _store.ManagerName;
			FindViewById<EditText>(Resource.Id.address1).Text = _store.Address1;
			FindViewById<EditText>(Resource.Id.address2).Text = _store.Address2;
			FindViewById<EditText>(Resource.Id.zip).Text = _store.Zip.ToString();
			FindViewById<EditText>(Resource.Id.city).Text = _store.City;
			FindViewById<EditText>(Resource.Id.state).Text = _store.State;
			FindViewById<EditText>(Resource.Id.phoneNum).Text = _store.PhoneNum;
			FindViewById<EditText>(Resource.Id.territoryNum).Text = _store.TerritoryNum.ToString();
		}

		private Store UpdateStore()
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

			Store store = new Store(_store.StoreID, storeName, storeNum, sequenceNum, managerName, 
				phoneNum, address1, address2, city, state, zip, territoryNum);
		
			_database.UpdateStore(store);
			
			return store;
		}
	}
}