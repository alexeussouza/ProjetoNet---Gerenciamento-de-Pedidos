namespace backend.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }

        // FK para o pedido
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        // FK para o produto
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;

        public int Quantidade { get; set; }
    }
}
