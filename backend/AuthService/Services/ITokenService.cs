namespace AuthService.Services
{
    public interface ITokenService
    {
        string GenerateToken(int userId, string email);

        // Novos métodos para hash e verificação de senha
        string HashPassword(string password);

        bool VerifyPassword(string password, string passwordHash);
    }
}