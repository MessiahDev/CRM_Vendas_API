using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Context;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Deal> Deals, List<Lead> Leads, List<Interaction> Interactions, List<Customer> Customers)> GetAllAsync()
        {
            var leads = await _context.Leads.AsNoTracking().ToListAsync();
            var interactions = await _context.Interactions.AsNoTracking()
                                                            .Include(c => c.Customer)
                                                            .Select(i => new Interaction
                                                            {
                                                                Id = i.Id,
                                                                Type = i.Type,
                                                                Notes = i.Notes,
                                                                Date = i.Date,
                                                                LeadId = i.LeadId,
                                                                Lead = new Lead
                                                                {
                                                                    Id = i.Lead.Id,
                                                                    Name = i.Lead.Name
                                                                },
                                                                CustomerId = i.CustomerId,
                                                                Customer = new Customer
                                                                {
                                                                    Id = i.Customer.Id,
                                                                    Name = i.Customer.Name
                                                                }
                                                            })
                                                            .ToListAsync();
            var customers = await _context.Customers.AsNoTracking().ToListAsync();
            var deals = await _context.Deals.AsNoTracking()
                                            .Include(d => d.Customer)
                                            .Select(d => new Deal
                                            {
                                                Id = d.Id,
                                                Value = d.Value,
                                                Stage = d.Stage,
                                                Customer = new Customer
                                                {
                                                    Id = d.Customer.Id,
                                                    Name = d.Customer.Name
                                                }
                                            })
                                            .ToListAsync();

            return (deals, leads, interactions, customers);
        }
    }
}
