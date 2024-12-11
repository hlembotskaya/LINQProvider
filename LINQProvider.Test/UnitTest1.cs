using LINQProvider.Models;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System.Linq;

namespace LINQProvider.Test
{
    public class Tests
    {
        SqlProvider sqlProvider;
        SqlQueryable<Product> queryable;
        string expectedSql;

        [SetUp]
        public void Setup()
        {
            expectedSql = "SELECT * FROM Products WHERE ((UnitPrice > 100) AND (Type = ’Electronics’))";
            sqlProvider = new SqlProvider(@"Server=localhost\SQLEXPRESS;Database=ProfileSample;Trusted_Connection=True;");
            queryable = new SqlQueryable<Product>(sqlProvider);
        }

        [Test]
        public void WithProvider()
        {
            var result = queryable.Where(p => p.UnitPrice > 100 && p.Type == "'Electronics'");
            Assert.AreEqual(result.First().Name, "Smartphone");
            Assert.AreEqual(result.First().UnitPrice, "450");

            //Name: Smartphone, 299
            //Name: Headphones, 199
            //Name: Laptop, 799
            //Name: Tablet, 450
  
        }
    }
}