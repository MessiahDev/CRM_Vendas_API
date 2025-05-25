using CRM_Vendas.Domain.Entities;

namespace CRM_Vendas.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistsAsync(int id);
        Task<User?> GetByResetTokenAsync(string token);
    }
}
