using System.Collections.Generic;

namespace backend.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty; // Nunca salve senha em texto puro em produção!

        // Relação 1:N — Um usuário pode ter vários pedidos
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
