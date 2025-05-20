using CRM_Vendas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM_Vendas_API.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Lead> Leads { get; set; } = null!;
        public DbSet<Interaction> Interactions { get; set; } = null!;
        public DbSet<Deal> Deals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Leads)
                .WithOne(l => l.Customer)
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Interactions)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Deals)
                .WithOne(d => d.Customer)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lead>()
                .HasMany(l => l.Interactions)
                .WithOne(i => i.Lead)
                .HasForeignKey(i => i.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lead>()
                .HasMany(l => l.Deals)
                .WithOne(d => d.Lead)
                .HasForeignKey(d => d.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Leads)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Customers)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
