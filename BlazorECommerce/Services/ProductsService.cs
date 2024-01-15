using BlazorECommerce.Models;
using BlazorECommerce.Services.IService;
using Newtonsoft.Json;
using System.Text;

namespace BlazorECommerce.Services
{
    public class ProductsService : IProduct
    {
        private readonly HttpClient _httpClient;
        private readonly string BASEURL = "https://localhost:7203";
        public ProductsService(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }
        public async Task<ResponseDto> AddProduct(ProductRequestDto productRequestDto)
        {
            var request = JsonConvert.SerializeObject(productRequestDto);
            var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BASEURL}/api/Product", bodyContent);
            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (results.IsSuccess)
            {
                //change this to a list of products
                return results;

            }
        
            return new ResponseDto();
        }

        public async Task<ResponseDto> DeleteProduct(Guid Id)
        {
            var response = await _httpClient.DeleteAsync($"{BASEURL}/api/Product/{Id}");
            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (results.IsSuccess)
            {
                //change this to a list of products
                return results;

            }

            return new ResponseDto();
        }

        public async Task<Product> GetProductById(Guid Id)
        {
            var response = await _httpClient.GetAsync($"{BASEURL}/api/Product/{Id}");
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return JsonConvert.DeserializeObject<Product>(results.Result.ToString());

            }
            return new Product();
        }

        public async Task<List<Product>> GetProducts()
        {
            var response = await _httpClient.GetAsync($"{BASEURL}/api/Product");
            var content = await response.Content.ReadAsStringAsync();


            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return JsonConvert.DeserializeObject<List<Product>>(results.Result.ToString());

            }
            return new List<Product>();
        }

        public async Task<ResponseDto> UpdateProduct(Guid id, ProductRequestDto productRequestDto)
        {
            var request = JsonConvert.SerializeObject(productRequestDto);
            var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{BASEURL}/api/Product/{id}", bodyContent);
            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (results.IsSuccess)
            {
                //change this to a list of products
                return results;

            }

            return new ResponseDto();
        }
    }
    
}
