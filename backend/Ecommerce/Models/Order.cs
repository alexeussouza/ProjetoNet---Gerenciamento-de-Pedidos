using Ecommerce.Enums;
using System;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Received; // Default status

        // Foreign Key - User
        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
