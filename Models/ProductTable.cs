using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;


namespace KhumaloCrafts1.Models
{
	public class productTable
	{
		public static string con_string = "Server=tcp:webappsources.database.windows.net,1433;Initial Catalog=webapp1;Persist Security Info=False;User ID=kheni;Password=Khenende@1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

		public int ProductID { get; set; }
		public string Name { get; set; }
		public string Price { get; set; }
		public string Category { get; set; }
		public string Availability { get; set; }

		internal static List<productTable> AllProducts
		{
			get
			{
				return GetAllProducts();
			}
		}

		public int InsertProduct(productTable p)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(con_string))
				{
					string sql = "INSERT INTO productTable (productName, productPrice, productCategory, productAvailability) VALUES (@Name, @Price, @Category, @Availability)";
					using (SqlCommand cmd = new SqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@Name", p.Name);
						cmd.Parameters.AddWithValue("@Price", p.Price);
						cmd.Parameters.AddWithValue("@Category", p.Category);
						cmd.Parameters.AddWithValue("@Availability", p.Availability);

						con.Open();
						int rowsAffected = cmd.ExecuteNonQuery();
						con.Close();

						return rowsAffected;
					}
				}
			}
			catch (Exception ex)
			{
				// Log the exception or handle it appropriately
				throw new Exception("An error occurred while inserting the product.", ex);
			}
		}

		// Method to retrieve all products from the database
		public static List<productTable> GetAllProducts()
		{
			List<productTable> products = new List<productTable>();

			try
			{
				using (SqlConnection con = new SqlConnection(con_string))
				{
					string sql = "SELECT * FROM productTable";
					using (SqlCommand cmd = new SqlCommand(sql, con))
					{
						con.Open();
						using (SqlDataReader rdr = cmd.ExecuteReader())
						{
							while (rdr.Read())
							{
								productTable product = new productTable
								{
									ProductID = Convert.ToInt32(rdr["productID"]),
									Name = rdr["productName"].ToString(),
									Price = rdr["productPrice"].ToString(),
									Category = rdr["productCategory"].ToString(),
									Availability = rdr["productAvailability"].ToString()
								};

								products.Add(product);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Log the exception or handle it appropriately
				throw new Exception("An error occurred while retrieving products.", ex);
			}

			return products;
		}
	}
}