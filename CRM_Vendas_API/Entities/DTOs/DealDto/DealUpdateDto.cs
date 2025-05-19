using CRM_Vendas_API.Entities.Models.Enums;

namespace CRM_Vendas_API.Entities.DTOs.DealDto
{
    public class DealUpdateDto
    {
        public string Title { get; set; } = null!;
        public decimal Value { get; set; }
        public DealStage Stage { get; set; } = DealStage.Novo;
    }
}
