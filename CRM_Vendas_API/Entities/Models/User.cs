using System.ComponentModel.DataAnnotations;

namespace CRM_Vendas_API.Entities.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
