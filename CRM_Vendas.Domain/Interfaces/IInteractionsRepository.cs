using CRM_Vendas.Domain.Entities;

namespace CRM_Vendas.Domain.Interfaces
{
    public interface IInteractionsRepository
    {
        Task<IEnumerable<Interaction>> GetAllAsync();
        Task<Interaction?> GetByIdAsync(int id);
        Task AddAsync(Interaction interaction);
        Task UpdateAsync(Interaction interaction);
        Task DeleteAsync(Interaction interaction);
        Task<bool> ExistsAsync(int id);
    }
}
