using CRM_Vendas.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CRM_Vendas.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public string PasswordHash { get; set; } = null!;

        public string? PasswordResetToken { get; set; }

        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
