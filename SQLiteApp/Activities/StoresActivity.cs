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
	public class StoresActivity : Activity
	{
		protected ListView _storeList;
		protected EditText _storeIdEditText;
		protected Database _database;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Stores);
			ActionBar.SetDisplayShowHomeEnabled(false);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
		}

		protected void SyncDatabase()
		{
			var dbName = DatabaseName();
			if (!File.Exists(dbName))
			{
				CopyDatabaseFromAsset("Stores.sqlite");
			}
		}

		protected virtual void GetUIComponents()
		{
			_storeList = (ListView)FindViewById(Resource.Id.storeList);
			
			_storeList.ItemClick += (s, e) =>
			{
				var adapter = (StoreNameAdapter)_storeList.Adapter;
				var store = adapter[e.Position];
				
				ViewStoreDetails(store);
			};
		}

		protected string DatabaseName()
		{
			var dir = Android.OS.Environment.ExternalStorageDirectory.ToString();
			var filename = "Stores.sqlite";
			return dir + "/" + filename;
		}

		protected void ViewStoreDetails(Store store)
		{
			Intent storeDetailIntent = new Intent(this, typeof(StoreDetailActivity));
			storeDetailIntent.PutExtra("Store", JsonConvert.SerializeObject(store));
			StartActivity(storeDetailIntent);
		}

		protected void CopyDatabaseFromAsset(string filename)
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

		protected virtual void PopulateView()
		{
			Store[] allStores = _database.GetAllStores();

			StoreNameAdapter adapter = new StoreNameAdapter(this, allStores);
			_storeList.Adapter = adapter;
		}
	}
}

