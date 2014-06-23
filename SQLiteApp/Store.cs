namespace SQLiteApp
{
	class Store
	{
		public string StoreID { get; set; }
		public string StoreName { get; set; }
		public string StoreNum { get; set; }
		public string SequenceNum { get; set; }
		public string ManagerName { get; set; }
		public string PhoneNum { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public int Zip { get; set; }
		public int TerritoryNum { get; set; }
		public long TotalOrders { get; set; }
		public double TotalExpenses { get; set; }

		public Store(string storeID, string storeName, string storeNum, string sequenceNum, string managerName, string phoneNum,
			string address1, string address2, string city, string state, int zip, int territoryNum, long totalOrders, double totalExpenses)
		{
			StoreID = storeID;
			StoreName = storeName;
			StoreNum = storeNum;
			SequenceNum = sequenceNum;
			ManagerName = managerName;
			PhoneNum = phoneNum;
			Address1 = address1;
			Address2 = address2;
			City = city;
			State = state;
			Zip = zip;
			TerritoryNum = territoryNum;
			TotalOrders = totalOrders;
			TotalExpenses = totalExpenses;
		}
	}
}