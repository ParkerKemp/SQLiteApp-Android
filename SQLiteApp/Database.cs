using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace SQLiteApp
{
	class Database
	{
		private string _dbPath;

		public Database(string dbPath)
		{
			_dbPath = dbPath;
		}

		public Store[] GetAllStores()
		{
			SqliteCommand command = new SqliteCommand("SELECT S.*, Count(O.OrderID), Sum(O.TotalCost) FROM Stores AS S, Orders AS O WHERE S.StoreID = O.StoreID GROUP BY S.StoreID");
			return GetStoresFromCommand(command);
		}

		public Store[] SearchStores(string searchTerm)
		{
			SqliteCommand command = new SqliteCommand("SELECT S.*, Count(O.OrderID), Sum(O.TotalCost) FROM Stores AS S, Orders AS O WHERE S.StoreName LIKE @SearchTerm AND S.StoreID = O.StoreID GROUP BY S.StoreID");
			command.Parameters.Add(new SqliteParameter("@SearchTerm", "%" + searchTerm + "%"));
			return GetStoresFromCommand(command);
		}

		private Store[] GetStoresFromCommand(SqliteCommand command)
		{
			List<Store> ret = new List<Store>();
			
			using (SqliteConnection connection = new SqliteConnection("Data Source=" + _dbPath + ";Version=3"))
			{
				connection.Open();
				command.Connection = connection;

				SqliteDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					try
					{
						ret.Add(new Store((string)reader["StoreID"], (string)reader["StoreName"], (string)reader["StoreNum"], (string)reader["SequenceNum"],
										(string)reader["ManagerName"], (string)reader["PhoneNum"], (string)reader["Address1"], (string)reader["Address2"],
										(string)reader["City"], (string)reader["State"], (int)reader["Zip"], (int)reader["TerritoryNum"],
										(long)reader["Count(O.OrderID)"], (double)reader["Sum(O.TotalCost)"]));
					}
					catch (Exception e)
					{
						Console.WriteLine(e.StackTrace);
						Console.WriteLine(e.Message);
						throw;
					}
				}
			}
			return ret.ToArray();
		}

		public Order[] GetOrdersForStore(string storeID)
		{
			List<Order> ret = new List<Order>();
			string query = "SELECT O.*, S.StoreName FROM Orders AS O, Stores AS S WHERE O.StoreID = @StoreID AND O.StoreID = S.StoreID";

			using (SqliteConnection connection = new SqliteConnection("Data Source=" + _dbPath + ";Version=3"))
			{
				connection.Open();
				SqliteCommand command = new SqliteCommand(query, connection);
				command.Parameters.Add(new SqliteParameter("@StoreID", storeID));

				SqliteDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					try
					{
						ret.Add(new Order((string)reader["StoreID"], (string)reader["StoreName"], (string)reader["OrderID"], (string)reader["Date"], (int)reader["TotalItems"],
										(double)reader["TotalCost"], (string)reader["ContactName"], reader["RushOrder"].ToString().Equals("True")));
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						throw;
					}
				}
			}
			return ret.ToArray();
		}
	}
}