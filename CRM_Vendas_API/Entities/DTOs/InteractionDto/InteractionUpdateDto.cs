using System.ComponentModel.DataAnnotations;

namespace CRM_Vendas_API.Entities.DTOs.InteractionDto
{
    public class InteractionUpdateDto
    {
        public string Type { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime? Date { get; set; }
    }
}
