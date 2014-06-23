package sqliteapp;


public class StoreTable
	extends android.widget.TableLayout
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("SQLiteApp.StoreTable, SQLiteApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", StoreTable.class, __md_methods);
	}


	public StoreTable (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == StoreTable.class)
			mono.android.TypeManager.Activate ("SQLiteApp.StoreTable, SQLiteApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public StoreTable (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == StoreTable.class)
			mono.android.TypeManager.Activate ("SQLiteApp.StoreTable, SQLiteApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
