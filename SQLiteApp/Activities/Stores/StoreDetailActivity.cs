using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SQLiteApp.Activities;
using System;

namespace SQLiteApp
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	class StoreDetailActivity : SQLiteAppActivity
	{
		private Store _store;
		private Button _ordersButton;
		private Database _database;
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.StoreDetails);

			GetExtras();

			GetUIComponents();

			ActionBar.Title = _store.StoreName;
			ActionBar.SetHomeButtonEnabled(true);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetDisplayShowHomeEnabled(false);

			GetDatabase();
			
			PopulateView();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.EditDeleteMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					break;
				case Resource.Id.edit:
					EditStore();
					break;
				case Resource.Id.delete:
					ConfirmDelete();
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (resultCode == Result.Ok)
			{
				_store = JsonConvert.DeserializeObject<Store>(data.GetStringExtra("Store"));
			}
		}
		
		private void GetDatabase()
		{
			var dir = Android.OS.Environment.ExternalStorageDirectory.ToString();
			string filename = "Stores.sqlite";
			var dbName = dir + "/" + filename;

			_database = new Database(dbName);
		}

		private void EditStore()
		{
			Intent editStoreIntent = new Intent(this, typeof(EditStoreActivity));
			editStoreIntent.PutExtra("Store", JsonConvert.SerializeObject(_store));
			StartActivityForResult(editStoreIntent, 1);
		}

		private void ConfirmDelete()
		{
			AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle("Delete this store?");
			alert.SetPositiveButton("Yes", (s, a) =>
			{
				DeleteStore();
			});

			alert.SetNegativeButton("No", (s, a) =>
			{
			});

			alert.Show();
		}

		private void DeleteStore()
		{
			_database.DeleteStore(_store.StoreID);

			Toast.MakeText(ApplicationContext, "Store deleted.", ToastLength.Short).Show();
			ReturnEdited(true);
		}

		private void GetExtras()
		{
			_store = JsonConvert.DeserializeObject<Store>(Intent.GetStringExtra("Store"));
		}

		private void GetUIComponents()
		{
			_ordersButton = (Button)FindViewById(Resource.Id.totalOrdersButton);

			_ordersButton.Click += (s, e) =>
			{
				ViewOrders();
			};
		}

		private void ViewOrders()
		{
			Intent ordersIntent = new Intent(this, typeof(AllOrdersActivity));
			ordersIntent.PutExtra("StoreID", _store.StoreID);
			ordersIntent.PutExtra("StoreName", _store.StoreName);
			StartActivityForResult(ordersIntent, 1);
		}

		protected override void PopulateView()
		{
			_database.GetOrderOverview(_store);

			Console.WriteLine("Populating store details...");

			FindViewById<TextView>(Resource.Id.storeID).Text = _store.StoreID;
			FindViewById<TextView>(Resource.Id.territoryNum).Text = _store.TerritoryNum.ToString();
			FindViewById<TextView>(Resource.Id.sequenceNum).Text = _store.SequenceNum;
			FindViewById<TextView>(Resource.Id.address).Text = _store.Address1 + "\n" + _store.Address2
															+ "\n" + _store.City + ", " + _store.State + " " + _store.Zip.ToString();
			FindViewById<TextView>(Resource.Id.storeNum).Text = _store.StoreNum;
			FindViewById<TextView>(Resource.Id.managerName).Text = _store.ManagerName;
			FindViewById<TextView>(Resource.Id.phoneNum).Text = _store.PhoneNum;
			FindViewById<Button>(Resource.Id.totalOrdersButton).Text = _store.TotalOrders.ToString();
			FindViewById<TextView>(Resource.Id.totalExpenses).Text = "$" + string.Format("{0:0.00}", _store.TotalExpenses);
		}
	}
}