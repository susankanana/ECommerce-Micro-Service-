using CartService.Models.Dtos;
using CartService.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CartService.Services
{
    public class ProductsService : IProduct
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ProductDto> GetProductById(Guid Id )
        {

            var client = _httpClientFactory.CreateClient("Products");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync(Id.ToString());
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto.Result != null && response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
            }
            return new ProductDto();
        }
    }
}
