using System.ComponentModel.DataAnnotations;

namespace CRM_Vendas_API.Entities.Models
{
    public class Interaction
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; } = null!;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int? LeadId { get; set; }
        public Lead? Lead { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
