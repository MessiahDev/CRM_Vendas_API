using CRM_Vendas.Domain.Entities;

namespace CRM_Vendas.Domain.Interfaces
{
    public interface ILeadRepository
    {
        Task<IEnumerable<Lead>> GetAllAsync();
        Task<Lead?> GetByIdAsync(int id);
        Task<Lead?> GetByEmailAsync(string email);
        Task AddAsync(Lead lead);
        Task UpdateAsync(Lead lead);
        Task DeleteAsync(Lead lead);
        Task<bool> ExistsAsync(int id);
    }
}
