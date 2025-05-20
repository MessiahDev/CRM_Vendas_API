using System.ComponentModel.DataAnnotations;

namespace CRM_Vendas.Application.DTOs.UserDto
{
    public class UserLoginDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
