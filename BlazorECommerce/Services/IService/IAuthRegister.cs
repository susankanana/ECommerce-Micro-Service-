using BlazorECommerce.Models;

namespace BlazorECommerce.Services.IService
{
    public interface IAuthRegister
    {
        Task<ResponseDto> Register(RegisterUser registerRequestDto);
    }
}
