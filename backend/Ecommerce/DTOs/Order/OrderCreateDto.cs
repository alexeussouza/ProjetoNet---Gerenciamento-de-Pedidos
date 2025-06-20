namespace Ecommerce.DTOs.Order
{
    public class OrderCreateDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
