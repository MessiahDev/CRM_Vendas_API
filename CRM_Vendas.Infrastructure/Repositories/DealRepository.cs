using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Context;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas.Infrastructure.Repositories
{
    public class DealRepository : IDealRepository
    {
        private readonly AppDbContext _context;

        public DealRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Deal>> GetAllAsync()
        {
            return await _context.Deals
                .Include(i => i.Customer)
                .Include(i => i.Lead)
                .ToListAsync();
        }

        public async Task<Deal?> GetByIdAsync(int id)
        {
            return await _context.Deals
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(Deal deal)
        {
            await _context.Deals.AddAsync(deal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Deal deal)
        {
            _context.Deals.Update(deal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Deal deal)
        {
            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Deals.AnyAsync(d => d.Id == id);
        }
    }
}
