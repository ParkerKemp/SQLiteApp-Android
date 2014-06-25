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

		private void SearchStores(string searchTerm)
		{
			Intent storeSearchIntent = new Intent(this, typeof(SearchStoresActivity));
			storeSearchIntent.PutExtra("SearchTerm", searchTerm);
			StartActivity(storeSearchIntent);
		}
	}
}