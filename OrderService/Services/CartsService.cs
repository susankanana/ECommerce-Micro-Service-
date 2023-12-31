using Newtonsoft.Json;
using OrderService.Models.Dtos;
using OrderService.Services.IServices;
using System.Net.Http;

namespace OrderService.Services
{
    public class CartsService : ICart
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CartsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CartDto> GetCartById(Guid cartId)
        {
            var client = _httpClientFactory.CreateClient("Carts");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync(cartId.ToString());
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto.Result != null && response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<CartDto>(responseDto.Result.ToString());
            }
            return new CartDto();
        }
    }
}
