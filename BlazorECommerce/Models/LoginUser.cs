using System.ComponentModel.DataAnnotations;

namespace BlazorECommerce.Models
{
    public class LoginUser
    {
        
        public string UserName { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;
    }
}
