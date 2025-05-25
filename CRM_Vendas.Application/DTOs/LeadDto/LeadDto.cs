using CRM_Vendas_API.Entities.Models.Enums;

namespace CRM_Vendas_API.Entities.DTOs.LeadDto
{
    public class LeadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Source { get; set; }
        public LeadStatus Status { get; set; } = LeadStatus.Novo;
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
    }
}
