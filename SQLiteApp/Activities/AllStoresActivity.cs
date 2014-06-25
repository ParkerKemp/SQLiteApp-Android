using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.IO;

namespace SQLiteApp
{
	[Activity(MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, Theme = "@android:style/Theme.NoTitleBar")]
	public class AllStoresActivity : Activity
	{
		ListView _storeList;
		EditText _storeIdEditText;
		Database _database;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.AllStores);

			GetUIComponents();

			SyncDatabase();
			
			string searchTerm = Intent.GetStringExtra("SearchTerm");

			if (searchTerm != null)
			{
				_storeIdEditText.Visibility = ViewStates.Invisible;
				
				TextView searchPrompt = FindViewById<TextView>(Resource.Id.searchPrompt);
				searchPrompt.Text = "Searching for stores matching \"" + searchTerm + "\"...";
				searchPrompt.TextSize = 24;
				searchPrompt.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
				PopulateWithSearch(searchTerm);
			}
			else
				PopulateWithAll();

			_storeIdEditText.EditorAction += (s, e) => 
			{
				SearchStores(_storeIdEditText.Text);
			};
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
		}

		private void SyncDatabase()
		{
			var dir = Android.OS.Environment.ExternalStorageDirectory.ToString();
			string filename = "Stores.sqlite";
			var dbName = dir + "/" + filename;

			if (!File.Exists(dbName))
			{
				CopyDatabaseFromAsset(filename);
			}
			_database = new Database(dbName);
		}

		private void GetUIComponents()
		{
			_storeList = (ListView)FindViewById(Resource.Id.storeList);
			_storeIdEditText = (EditText)FindViewById(Resource.Id.storeIdEditText);

			_storeList.ItemClick += (s, e) =>
			{
				var adapter = (StoreNameAdapter)_storeList.Adapter;
				var store = adapter[e.Position];
				var json = JsonConvert.SerializeObject(store);
				Console.WriteLine(json);

				ViewStoreDetails(store);
			};
		}

		private void ViewStoreDetails(Store store)
		{
			Intent storeDetailIntent = new Intent(this, typeof(StoreDetailActivity));
			storeDetailIntent.PutExtra("Store", JsonConvert.SerializeObject(store));
			StartActivity(storeDetailIntent);
		}

		private void SearchStores(string search)
		{
			Intent storeSearchIntent = new Intent(this, typeof(AllStoresActivity));
			storeSearchIntent.PutExtra("SearchTerm", search);
			StartActivity(storeSearchIntent);
		}

		private void CopyDatabaseFromAsset(string filename)
		{
			try
			{
				var dir = Android.OS.Environment.ExternalStorageDirectory.ToString();
				using (Stream input = Assets.Open(filename))
				using (Stream output = new FileStream(dir + "/" + filename, FileMode.OpenOrCreate))
				{
					byte[] buffer = new byte[1024];
					int length;
					while ((length = input.Read(buffer, 0, 1024)) > 0)
						output.Write(buffer, 0, length);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				throw;
			}
		}

		private void PopulateWithAll()
		{
			Store[] allStores = _database.GetAllStores();

			StoreNameAdapter adapter = new StoreNameAdapter(this, allStores);
			_storeList.Adapter = adapter;
		}

		private void PopulateWithSearch(string searchTerm)
		{
			Store[] stores = _database.SearchStores(searchTerm);

			StoreNameAdapter adapter = new StoreNameAdapter(this, stores);
			_storeList.Adapter = adapter;
		}
	}
}

