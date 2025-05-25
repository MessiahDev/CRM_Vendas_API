namespace CRM_Vendas.Application.DTOs.UserDto
{
    public class UserResetPasswordDto
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
