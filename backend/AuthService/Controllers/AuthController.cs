using AuthService.Data;
using AuthService.Services;
using Ecommerce.DTOs.Auth;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AuthDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    // Endpoint para registrar usuário
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Verifica se email já está cadastrado
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest(new { message = "Email already registered." });

        // Cria novo usuário com senha hash
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = _tokenService.HashPassword(dto.Password) // método que deve ser implementado
        };

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while creating the user." });
        }

        // Retorna dados do usuário criado
        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }

    // Endpoint de login que gera o token JWT
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password." });

        // Verifica a senha (comparar hash)
        if (!_tokenService.VerifyPassword(dto.Password, user.PasswordHash)) // método que deve ser implementado
            return Unauthorized(new { message = "Invalid email or password." });

        // Gera token
        var token = _tokenService.GenerateToken(user.Id, user.Email);

        return Ok(new { Token = token });
    }

    // Endpoint para obter perfil do usuário autenticado
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        // Obtém email do usuário autenticado via Claims (geralmente ClaimTypes.Email)
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email == null)
            return Unauthorized(new { message = "Invalid token or not authenticated." });

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return NotFound(new { message = "User not found." });

        return Ok(new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }
}
