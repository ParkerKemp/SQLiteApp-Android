using Android.App;
using Android.Views;
using Android.Widget;

namespace SQLiteApp
{
	class StoreNameAdapter : BaseAdapter<Store>
	{
		Store[] _items;
		Activity _context;

		public StoreNameAdapter(Activity context, Store[] items)
			: base()
		{
			_context = context;
			_items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override int Count
		{
			get 
			{
				return _items.Length;
			}
		}

		public override Store this[int position]
		{
			get { return _items[position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view = _context.LayoutInflater.Inflate(Resource.Layout.StoreListItem, null);
			view.FindViewById<TextView>(Resource.Id.storeName).Text = _items[position].StoreName;
			return view;
		}
	}
}