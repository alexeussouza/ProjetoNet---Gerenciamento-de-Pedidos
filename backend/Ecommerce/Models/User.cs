using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class User
    {
        public int Id { get; set; }             // User identifier (Primary Key)
        public string Name { get; set; } = string.Empty;       // User full name
        public string Email { get; set; } = string.Empty;      // User email
        public string PasswordHash { get; set; } = string.Empty; // 🔐 Armazena o hash da senha



        // Navigation property for Orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
