using System.Collections.Generic;

namespace backend.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        // Relação 1:N — Um produto pode estar em vários itens de pedidos
        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
