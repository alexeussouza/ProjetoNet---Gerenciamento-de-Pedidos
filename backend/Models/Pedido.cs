using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        // FK para o usuário que fez o pedido
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string Status { get; set; } = "Recebido";

        public DateTime DataPedido { get; set; } = DateTime.UtcNow;

        // Relação 1:N — Um pedido pode ter vários itens
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
