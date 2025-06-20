using ProductService.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Configura o DbContext com PostgreSQL
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔐 Configura autenticação JWT
var key = builder.Configuration["Jwt:Key"]!;
Console.WriteLine($"JWT Key carregada ProductService: {key}");  // debug provisório pra validar leitura no container
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// 🌐 Configura CORS para aceitar chamadas do frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin() // Para desenvolvimento; em produção use .WithOrigins("http://seusite.com")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 🔒 Adiciona autorização
builder.Services.AddAuthorization();

// 🚩 Adiciona suporte a Controllers
builder.Services.AddControllers();

var app = builder.Build();

// 🔧 Middlewares

// 🌐 Habilita CORS (tem que vir antes do UseAuthorization)
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// 🚀 Mapeia os endpoints dos controllers
app.MapControllers();

app.Run();

