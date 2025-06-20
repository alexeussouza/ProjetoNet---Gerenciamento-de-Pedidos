using OrderService.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 📦 Configura o DbContext com PostgreSQL
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🌐 Habilita CORS para aceitar requisições do frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()   // ⚠️ Em produção, prefira .WithOrigins("https://seu-frontend.com")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 🔧 Registrar HttpClient para comunicação entre serviços
builder.Services.AddHttpClient();

// 📦 Adiciona controllers
builder.Services.AddControllers();

// 🔐 Configura autenticação JWT
var key = builder.Configuration["Jwt:Key"]!;
Console.WriteLine($"JWT Key carregada OrderService: {key}");  // debug provisório pra validar leitura no container
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

builder.Services.AddAuthorization();

var app = builder.Build();

// 🔧 Middlewares
app.UseCors();          // <--- Importante: antes de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
