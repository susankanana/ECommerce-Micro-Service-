using BlazorECommerce.Models;

namespace BlazorECommerce.Services.IService
{
    public interface IAuthLogin
    {
        Task<LoginResponseDto> Login(LoginUser loginRequestDto);
    }
}
