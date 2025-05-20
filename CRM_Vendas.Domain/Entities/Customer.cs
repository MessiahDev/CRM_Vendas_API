using System.ComponentModel.DataAnnotations;

namespace CRM_Vendas.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public DateTime ConvertedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public ICollection<Lead> Leads { get; set; } = new List<Lead>();
        public ICollection<Deal> Deals { get; set; } = new List<Deal>();
    }
}
