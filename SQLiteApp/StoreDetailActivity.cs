using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
namespace SQLiteApp
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, Theme = "@android:style/Theme.NoTitleBar")]
	class StoreDetailActivity : Activity
	{
		Store _store;
		Button _ordersButton;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.StoreDetails);

			GetExtras();

			GetUIComponents();
			
			PopulateData();
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
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
			StartActivity(ordersIntent);
		}

		private void PopulateData()
		{
			FindViewById<TextView>(Resource.Id.storeName).Text = _store.StoreName;

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