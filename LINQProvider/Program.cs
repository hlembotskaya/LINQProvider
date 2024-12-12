using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using LINQProvider;
using LINQProvider.Models;

class Program
{
    static void Main()
    {
        var provider = new SqlProvider(@"Server=localhost\SQLEXPRESS;Database=ProfileSample;Trusted_Connection=True;");
        
        var queryable = new SqlQueryable<Product>(provider);
        var result = queryable.Where(p => p.UnitPrice > 100 && p.Type == "'Electronics'");

        var queryableFood = new SqlQueryable<FoodProduct>(provider);
        var foodResult = queryableFood.Where(p => p.Type == "'Vegetable'" && p.StockQuantity == 200);

        foreach (var product in result) {
            Console.WriteLine($"Name: {product.Name}, {product.UnitPrice}");
        }

        foreach (var product in foodResult)
        {
            Console.WriteLine($"Name: {product.Name}, {product.StockQuantity}");
        }

        //string connectionString = @"Server=localhost\SQLEXPRESS;Database=ProfileSample;Trusted_Connection=True;";
        //string query = "SELECT * FROM Products";
        //using (SqlConnection connection = new SqlConnection(connectionString))
        //{
        //    try
        //    {
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    Console.WriteLine($"ID: {reader["ID"]}, Name: {reader["ProductName"]}");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //}
    }
}
