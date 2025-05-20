namespace CRM_Vendas_API.Entities.DTOs.CustomerDto
{
    public class CustomerUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
