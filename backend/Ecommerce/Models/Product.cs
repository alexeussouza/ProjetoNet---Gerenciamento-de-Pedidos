using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Product
    {
        public int Id { get; set; }               // Product identifier
        public string Name { get; set; } = string.Empty;  // Product name
        public string Description { get; set; } = string.Empty; // Product description
        public decimal Price { get; set; }        // Product price

        // Navigation property for OrderItems (Not directly to Orders)
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
