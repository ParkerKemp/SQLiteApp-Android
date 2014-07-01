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
using System.IO;
using Newtonsoft.Json;

namespace SQLiteApp.Activities
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class SearchStoresActivity : StoresActivity
	{
		private string _searchTerm;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			_searchTerm = Intent.GetStringExtra("SearchTerm");

			GetUIComponents();

			FindViewById<EditText>(Resource.Id.storeIdEditText).Visibility = ViewStates.Invisible;
			
			_database = new Database(DatabaseName());

			ActionBar.Title = "Searching stores for \"" + _searchTerm + "\"...";

			ActionBar.SetHomeButtonEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);

			PopulateView();
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

		protected override void PopulateView()
		{
			Store[] stores = _database.SearchStores(_searchTerm);

			StoreNameAdapter adapter = new StoreNameAdapter(this, stores);
			FindViewById<ListView>(Resource.Id.storeList).Adapter = adapter;
		}
	}
}
