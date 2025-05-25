using CRM_Vendas.Domain.Enums;

namespace CRM_Vendas_API.Entities.DTOs.UserDto
{
    public class UserUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.User;
    }
}
