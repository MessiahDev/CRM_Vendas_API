namespace CRM_Vendas.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(string email, string password);
        string GenerateToken(string name, string email);
    }
}
