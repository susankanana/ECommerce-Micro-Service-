namespace BlazorECommerce.Models
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserResponseDto User { get; set; } = default!;
    }
}
