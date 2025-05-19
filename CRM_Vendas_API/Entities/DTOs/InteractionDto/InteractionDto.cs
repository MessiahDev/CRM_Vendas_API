namespace CRM_Vendas_API.Entities.DTOs.InteractionDto
{
    public class InteractionDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime Date { get; set; }
        public int? LeadId { get; set; }
        public int? CustomerId { get; set; }
    }
}
