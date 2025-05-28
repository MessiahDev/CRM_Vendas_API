using CRM_Vendas.Domain.Entities;

namespace CRM_Vendas.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<(List<Deal> Deals, List<Lead> Leads, List<Interaction> Interactions, List<Customer> Customers)> GetAllAsync();
    }
}
