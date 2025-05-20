using CRM_Vendas.Domain.Entities;
using CRM_Vendas.Domain.Interfaces;
using CRM_Vendas_API.Context;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task AddAsync(User user)
        {
            await _dbSet.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}