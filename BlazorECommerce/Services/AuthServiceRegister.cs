using BlazorECommerce.Models;
using BlazorECommerce.Services.IService;
using Newtonsoft.Json;
using System.Text;

namespace BlazorECommerce.Services
{
    public class AuthServiceRegister : IAuthRegister
    {
        private readonly HttpClient _httpClient;
        //private readonly string BASEURL = "https://thejitucommerceauthapi.azurewebsites.net";
        private readonly string BASEURL = "https://localhost:7115";
  
        public AuthServiceRegister(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDto> Register(RegisterUser registerRequestDto)
        {
            var request = JsonConvert.SerializeObject(registerRequestDto);
            var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");
            //communicate wih Api

            var response = await _httpClient.PostAsync($"{BASEURL}/api/User/Register", bodyContent);
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
