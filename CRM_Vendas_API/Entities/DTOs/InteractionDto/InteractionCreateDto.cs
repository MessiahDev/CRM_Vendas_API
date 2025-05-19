using System.ComponentModel.DataAnnotations;

namespace CRM_Vendas_API.Entities.DTOs.InteractionDto
{
    public class InteractionCreateDto
    {
        public string Type { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public int? LeadId { get; set; }
        public int? CustomerId { get; set; }
    }
}
