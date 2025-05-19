using System.ComponentModel.DataAnnotations;
using CRM_Vendas_API.Entities.Models.Enums;

namespace CRM_Vendas_API.Entities.DTOs.LeadDto
{
    public class LeadCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Source { get; set; }
        public LeadStatus Status { get; set; } = LeadStatus.Novo;
        public int UserId { get; set; }
        public int CustomerId { get; set; }
    }
}
