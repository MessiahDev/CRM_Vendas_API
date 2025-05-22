using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Context;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas.Infrastructure.Repositories
{
    public class InteractionsRepository : IInteractionsRepository
    {
        private readonly AppDbContext _context;

        public InteractionsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Interaction>> GetAllAsync()
        {
            return await _context.Interactions
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Interaction?> GetByIdAsync(int id)
        {
            return await _context.Interactions
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Interaction interaction)
        {
            await _context.Interactions.AddAsync(interaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Interaction interaction)
        {
            _context.Interactions.Update(interaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Interaction interaction)
        {
            _context.Interactions.Remove(interaction);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Interactions.AnyAsync(i => i.Id == id);
        }
    }
}
