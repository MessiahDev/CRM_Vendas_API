namespace CRM_Vendas_API.Entities.DTOs.CustomerDto
{
    public class CustomerCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int UserId { get; set; }
    }
}
