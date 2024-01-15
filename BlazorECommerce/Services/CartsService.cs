using BlazorECommerce.Models;
using BlazorECommerce.Services.IService;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace BlazorECommerce.Services
{
    public class CartsService : ICart
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly string BASEURL = "https://localhost:7097";
        public CartsService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public async Task<ResponseDto> AddProductToCart(AddCartItemDto cartItemDto)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            //var userId = await _localStorage.GetItemAsStringAsync("userId");
            //set authorization headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = JsonConvert.SerializeObject(cartItemDto);
            var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");
            //communicate wih Api

            var response = await _httpClient.PostAsync($"{BASEURL}/api/CartItem", bodyContent);
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return results;

            }
            return new ResponseDto();
        }

        public async Task<ResponseDto> ApplyCoupon(Cart cart ,string code)
        {
            var request = JsonConvert.SerializeObject(cart);
            var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");
            //communicate wih Api

            var response = await _httpClient.PutAsync($"{BASEURL}/api/Cart/coupon/{cart.UserId}?Code={code}", bodyContent);
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return results;

            }
            return new ResponseDto();
        }

        public async Task<Cart> GetCartByUserId(Guid userId)
        {
            var response = await _httpClient.GetAsync($"{BASEURL}/api/Cart/view/{userId}");
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return JsonConvert.DeserializeObject<Cart>(results.Result.ToString());

            }
            return new Cart();
        }

        public async Task<ResponseDto> ReduceCartItemQuantity(RemoveFromCartDto item)
        {
            var response = await _httpClient.DeleteAsync($"{BASEURL}/api/CartItem");
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return results;

            }
            return new ResponseDto();
        }

        public async Task<ResponseDto> RemoveProductFromCart(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"{BASEURL}/api/CartItem/{productId}");
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
