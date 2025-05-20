using CRM_Vendas.Domain.Entities;

namespace CRM_Vendas.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task SaveChangesAsync();
    }
}
