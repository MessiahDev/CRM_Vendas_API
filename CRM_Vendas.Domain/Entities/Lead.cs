using System.ComponentModel.DataAnnotations;
using CRM_Vendas_API.Entities.Models.Enums;

namespace CRM_Vendas.Domain.Entities
{
    public class Lead
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Source { get; set; }

        public LeadStatus Status { get; set; } = LeadStatus.Novo;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
        public ICollection<Deal> Deals { get; set; } = new List<Deal>();
    }
}
