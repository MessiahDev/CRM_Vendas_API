using CRM_Vendas_API.Entities.Models.Enums;

namespace CRM_Vendas_API.Entities.DTOs.DealDto
{
    public class DealCreateDto
    {
        public string Title { get; set; } = null!;
        public decimal Value { get; set; }
        public DealStage Stage { get; set; } = DealStage.Novo;
        public int CustomerId { get; set; }
        public int LeadId { get; set; }
    }
}
