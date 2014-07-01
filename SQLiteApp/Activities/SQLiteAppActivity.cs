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

namespace SQLiteApp.Activities
{
	public class SQLiteAppActivity : Activity
	{
		private bool _edited = false, _cascadeEdit = false;

		protected override void OnResume()
		{
			base.OnResume();
			if (_edited)
			{
				PopulateView();
				_edited = false;
			}
		}

		protected virtual void PopulateView()
		{
			Console.WriteLine("Error: empty virtual method called.");
		}

		protected void ReturnEdited(bool value)
		{
			Intent returnIntent = new Intent();
			returnIntent.PutExtra("Edited", _cascadeEdit || value);
			SetResult(Result.Ok, returnIntent);
			Finish();
		}

		protected void ReturnEdited(Intent returnIntent, bool value)
		{
			returnIntent.PutExtra("Edited", _cascadeEdit || value);
			SetResult(Result.Ok, returnIntent);
			Finish();
		}

		protected void ShortToast(string text)
		{
			Toast.MakeText(ApplicationContext, text, ToastLength.Short).Show();
		}

		public override void OnBackPressed()
		{
			Intent returnIntent = new Intent();
			returnIntent.PutExtra("Edited", _cascadeEdit);
			SetResult(Result.Canceled, returnIntent);
			base.OnBackPressed();
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			_edited |= data.GetBooleanExtra("Edited", false);
			_cascadeEdit = _edited;
		}
	}
}