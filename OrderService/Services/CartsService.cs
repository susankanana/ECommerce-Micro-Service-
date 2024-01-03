using Newtonsoft.Json;
using OrderService.Models.Dtos;
using OrderService.Services.IServices;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OrderService.Services
{
    public class CartsService : ICart
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CartsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CartDto> GetCartByUserId(Guid userId ,string token)
        {
            var client = _httpClientFactory.CreateClient("Carts");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(userId.ToString());
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto.Result != null && response.IsSuccessStatusCode)
            {
                var cart = JsonConvert.DeserializeObject<CartDto>(responseDto.Result.ToString());
                return cart;
            }
            return new CartDto();
        }

        public async Task<bool> DeleteCart(Guid userId , string token)
        {
            var cart = await GetCartByUserId(userId , token);
            var client = _httpClientFactory.CreateClient("Carts");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync(cart.CartId.ToString());
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
