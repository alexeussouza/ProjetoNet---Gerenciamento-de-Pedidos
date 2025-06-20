using Ecommerce.DTOs.Product;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;

namespace ProductService.Controllers;

[ApiController]
[Route("api/products")]  // <-- Rota no plural confore ApiServices do FrontEnd
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ProductDbContext _context;

    public ProductController(ProductDbContext context)
    {
        _context = context;
    }

    // Endpoint que lista todos os produtos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _context.Products
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                })
                .ToListAsync();

            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Endpoint para obter produto pelo ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid product ID. ID must be greater than zero.");
        }

        try
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Endpoint para criar produto
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Product data must be provided.");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Product name is required.");
        }

        try
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

