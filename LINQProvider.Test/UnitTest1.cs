using LINQProvider.Models;
using Microsoft.VisualBasic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LINQProvider.Test
{
    public class Tests
    {
        SqlProvider sqlProvider;
        SqlQueryable<FoodProduct> queryableFood;
        SqlQueryable<Product> queryableProduct;
        string expectedResult;
        string expectedResult1;

        [SetUp]
        public void Setup()
        {
            expectedResult = "SELECT * FROM Products WHERE((UnitPrice > 100) AND (Type = 'Electronics'))";
            expectedResult1 = "SELECT * FROM FoodProducts WHERE ((Type = 'Vegetable') AND (StockQuantity = 200))";
            sqlProvider = new SqlProvider(@"Server=localhost\SQLEXPRESS;Database=ProfileSample;Trusted_Connection=True;");
            


        }

        [Test]
        public void ProductTest()
        {
            Expression<Func<Product, bool>> expression = product => product.UnitPrice > 100 && product.Type == "'Electronics'";
            var queryable = new Product[] { }.AsQueryable().Where(expression);
            string result = sqlProvider.TranslateToSql<Product>(queryable.Expression);

            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        public void FoodProductProvider()
        {
            Expression<Func<FoodProduct, bool>> expression = foodProduct => foodProduct.Type == "'Vegetable'" && foodProduct.StockQuantity == 200;
            var queryable = new FoodProduct[] { }.AsQueryable().Where(expression);
            string result = sqlProvider.TranslateToSql<FoodProduct>(queryable.Expression);

            Assert.AreEqual(result, expectedResult1);
        }
    }
}