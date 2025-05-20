using System.ComponentModel.DataAnnotations;
using CRM_Vendas_API.Entities.Models.Enums;

namespace CRM_Vendas.Domain.Entities
{
    public class Deal
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }

        public DealStage Stage { get; set; } = DealStage.Novo;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public int LeadId { get; set; }
        public Lead Lead { get; set; } = null!;
    }
}
