using Android.App;
using Android.Views;
using Android.Widget;

namespace SQLiteApp
{
	class OrderSummaryAdapter : BaseAdapter<Order>
	{
		Order[] _items;
		Activity _context;

		public OrderSummaryAdapter(Activity context, Order[] items)
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

		public override Order this[int position]
		{
			get { return _items[position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
				view = _context.LayoutInflater.Inflate(Resource.Layout.OrderSummaryItem, null);
			view.FindViewById<TextView>(Resource.Id.cost).Text = "$" + string.Format("{0:0.00}", _items[position].TotalCost);
			view.FindViewById<TextView>(Resource.Id.date).Text = _items[position].Date;
			return view;
		}
	}
}