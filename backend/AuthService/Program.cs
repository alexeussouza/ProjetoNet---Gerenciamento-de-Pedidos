using AuthService.Data;
using AuthService.Services;
using BCrypt.Net;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.DTOs.Auth;
using DotNetEnv;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// 📦 Carrega variáveis do .env (se existir)
DotNetEnv.Env.Load();

// 🔑 JWT Key
var key = builder.Configuration["Jwt:Key"]
    ?? Environment.GetEnvironmentVariable("JWT_KEY");

if (string.IsNullOrEmpty(key))
    throw new Exception("JWT Key não configurada!");

// 📦 Conexão com PostgreSQL
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(connectionString));

// 📦 Serviços
builder.Services.AddScoped<ITokenService, TokenService>();

// 🔐 Autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

// 🌐 CORS liberado para desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// 📦 Aplica migrações no banco
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    db.Database.Migrate();
}

// 📦 Middlewares
app.UseCors("DevelopmentCors");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ✅ Endpoints OPTIONS para evitar 405 em preflight CORS
app.MapMethods("/api/auth/login", new[] { "OPTIONS" }, () =>
    Results.NoContent()).WithMetadata(new EnableCorsAttribute("DevelopmentCors"));

app.MapMethods("/api/auth/register", new[] { "OPTIONS" }, () =>
    Results.NoContent()).WithMetadata(new EnableCorsAttribute("DevelopmentCors"));

// 📦 Endpoint de registro
app.MapPost("/api/auth/register", async (UserRegisterDto request, AuthDbContext db) =>
{
    if (await db.Users.AnyAsync(u => u.Email == request.Email))
        return Results.BadRequest("Email já cadastrado.");

    var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

    var user = new User
    {
        Name = request.Name,
        Email = request.Email,
        PasswordHash = passwordHash
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok("Usuário registrado com sucesso.");
});

// 📦 Endpoint de login
app.MapPost("/api/auth/login", async (UserLoginDto request, AuthDbContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        return Results.Unauthorized();

    var token = GenerateJwtToken(user.Email, key);

    return Results.Ok(new { token });
});

// 📦 Rota protegida
app.MapGet("/api/auth/profile", (ClaimsPrincipal user) =>
{
    var email = user.Identity?.Name;
    return Results.Ok(new { Email = email, Message = "Acesso permitido à rota protegida." });
}).RequireAuthorization();

// 📦 Porta de escuta
app.Urls.Add("http://*:8080");
app.Run();

// 🔐 Gera token JWT
string GenerateJwtToken(string email, string key)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenKey = Encoding.UTF8.GetBytes(key);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }),
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(tokenKey),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}
