using CRM_Vendas.Domain.Entities;

namespace CRM_Vendas.Domain.Interfaces
{
    public interface IDealRepository
    {
        Task<IEnumerable<Deal>> GetAllAsync();
        Task<Deal?> GetByIdAsync(int id);
        Task AddAsync(Deal deal);
        Task UpdateAsync(Deal deal);
        Task DeleteAsync(Deal deal);
        Task<bool> ExistsAsync(int id);
    }
}
