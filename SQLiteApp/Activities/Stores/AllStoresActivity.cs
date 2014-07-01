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
using Newtonsoft.Json;

namespace SQLiteApp.Activities
{
	[Activity(Label = "All Stores", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class AllStoresActivity : StoresActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			GetUIComponents();

			SyncDatabase();

			_database = new Database(DatabaseName());

			PopulateView();
		}

		protected override void OnResume()
		{
			base.OnResume();

			_storeIdEditText.Text = "";
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.PlusMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.addNew:
					AddNewStore();
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		protected override void GetUIComponents()
		{
			base.GetUIComponents();
			_storeList = (ListView)FindViewById(Resource.Id.storeList);
			
			_storeIdEditText = FindViewById<EditText>(Resource.Id.storeIdEditText);

			_storeIdEditText.EditorAction += (s, e) =>
			{
				SearchStores(_storeIdEditText.Text);
			};
		}

		private void AddNewStore()
		{
			Intent addStoreIntent = new Intent(this, typeof(NewStoreActivity));
			StartActivityForResult(addStoreIntent, 1);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (requestCode == 1)
			{
				if (resultCode == Result.Ok)
				{
					Store store = JsonConvert.DeserializeObject<Store>(data.GetStringExtra("Store"));
					ViewStoreDetails(store);
				}
			}
		}

		private void SearchStores(string searchTerm)
		{
			Intent storeSearchIntent = new Intent(this, typeof(SearchStoresActivity));
			storeSearchIntent.PutExtra("SearchTerm", searchTerm);
			StartActivityForResult(storeSearchIntent, 2);
		}

		protected override void PopulateView()
		{
			Store[] allStores = _database.GetAllStores();

			StoreNameAdapter adapter = new StoreNameAdapter(this, allStores);
			_storeList.Adapter = adapter;
		}
	}
}