using Newtonsoft.Json;
using OrderService.Models.Dtos;
using OrderService.Services.IServices;

namespace OrderService.Services
{
    public class CartItemService : ICartItem
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CartItemService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CartItemDto> GetCartItemById(Guid cartItemId)
        {
            var client = _httpClientFactory.CreateClient("CartItems");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync(cartItemId.ToString());
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto.Result != null && response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<CartItemDto>(responseDto.Result.ToString());
            }
            return new CartItemDto();
        }
    }
}
