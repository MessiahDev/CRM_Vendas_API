using CRM_Vendas.Domain.Entities;

namespace CRM_Vendas_API.Entities.DTOs.CustomerDto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime ConvertedAt { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
