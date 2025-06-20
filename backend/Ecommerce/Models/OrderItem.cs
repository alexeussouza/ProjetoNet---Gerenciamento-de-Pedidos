namespace Ecommerce.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Foreign Key - Order
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // Foreign Key - Product
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; } // Price at the time of order
    }
}
