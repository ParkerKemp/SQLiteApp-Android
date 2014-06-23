namespace SQLiteApp
{
	class Order
	{
		public string StoreID { get; set; }
		public string StoreName { get; set; }
		public string OrderID { get; set; }
		public string Date { get; set; }
		public int TotalItems { get; set; }
		public double TotalCost { get; set; }
		public string ContactName { get; set; }
		public bool RushOrder { get; set; }

		public Order(string storeID, string storeName, string orderID, string date, int totalItems, double totalCost, string contactName, bool rushOrder)
		{
			StoreID = storeID;
			StoreName = storeName;
			OrderID = orderID;
			Date = date;
			TotalItems = totalItems;
			TotalCost = totalCost;
			ContactName = contactName;
			RushOrder = rushOrder;
		}
	}
}