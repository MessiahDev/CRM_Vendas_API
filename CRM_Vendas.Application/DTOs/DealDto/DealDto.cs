using CRM_Vendas_API.Entities.Models.Enums;

namespace CRM_Vendas_API.Entities.DTOs.DealDto
{
    public class DealDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Value { get; set; }
        public DealStage Stage { get; set; } = DealStage.Novo;
        public DateTime CreatedAt { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int LeadId { get; set; }
        public string? LeadName { get; set; }
    }
}
