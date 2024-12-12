using System;

namespace LINQProvider.Models
{
    public class FoodProduct
    {
        public int ID { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public int StockQuantity { get; set; }
    }
}
