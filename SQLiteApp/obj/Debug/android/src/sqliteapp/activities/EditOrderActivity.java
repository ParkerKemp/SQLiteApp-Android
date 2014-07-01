package sqliteapp.activities;


public class EditOrderActivity
	extends sqliteapp.activities.OrderEntryActivity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("SQLiteApp.Activities.EditOrderActivity, SQLiteApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EditOrderActivity.class, __md_methods);
	}


	public EditOrderActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EditOrderActivity.class)
			mono.android.TypeManager.Activate ("SQLiteApp.Activities.EditOrderActivity, SQLiteApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
