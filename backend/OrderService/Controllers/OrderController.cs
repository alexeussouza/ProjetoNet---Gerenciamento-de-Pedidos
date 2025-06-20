using Ecommerce.Enums;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using System.Security.Claims;
using Ecommerce.DTOs.Order;

namespace OrderService.Controllers;

[ApiController]
[Route("api/orders")]  // Rota no plural conforme ApiService do FrontEnd
[Authorize]
public class OrderController : ControllerBase
{
    private readonly OrderDbContext _context;
    private readonly ILogger<OrderController> _logger;

    public OrderController(OrderDbContext context, ILogger<OrderController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Endpoint que retorna todos os pedidos do usuário logado
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Obtém o ID do usuário autenticado via Claims
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Busca os pedidos desse usuário com os itens associados
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            })
            .ToListAsync();

        return Ok(orders);
    }

    // Endpoint para criar um novo pedido
    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateDto dto)
    {
        // Obtém ID do usuário logado
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Simula pagamento e reserva de estoque (aleatório para teste)
        var random = new Random();
        var paymentApproved = random.Next(0, 2) == 1;

        // Cria o pedido
        var order = new Ecommerce.Models.Order
        {
            UserId = userId,
            Status = OrderStatus.Received,
            CreatedAt = DateTime.UtcNow,
            Items = dto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Simula o fluxo de pagamento e reserva de estoque
        if (paymentApproved)
        {
            order.Status = OrderStatus.PaymentApproved;
            _logger.LogInformation($"Payment approved for order {order.Id}");
            _logger.LogInformation($"Stock reserved for order {order.Id}");
        }
        else
        {
            order.Status = OrderStatus.PaymentFailed;
            _logger.LogInformation($"Payment failed for order {order.Id}");
        }

        await _context.SaveChangesAsync();

        return Ok(new { order.Id, order.Status });
    }
}
