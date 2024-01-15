using BlazorECommerce.Models;
using BlazorECommerce.Services.IService;
using Newtonsoft.Json;
using System.Text;

namespace BlazorECommerce.Services
{
    public class CouponsService : ICoupon
    {
        private readonly HttpClient _httpClient;
        private readonly string BASEURL = "https://localhost:7127";
        public CouponsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseDto> AddCoupon(AddCouponDto newCoupon)
        {
            var request = JsonConvert.SerializeObject(newCoupon);
            var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");
            //communicate wih Api

            var response = await _httpClient.PostAsync($"{BASEURL}/api/Coupon", bodyContent);
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return results;
            }
            return new ResponseDto();
        }

        public async Task<ResponseDto> DeleteCoupon(Guid Id)
        {
            var response = await _httpClient.DeleteAsync($"{BASEURL}/api/Coupon/{Id}");
            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return results;
            }
            return new ResponseDto();
        }

        public async Task<List<Coupon>> GetAllCoupons()
        {
            var response = await _httpClient.GetAsync($"{BASEURL}/api/Coupon");
            var content = await response.Content.ReadAsStringAsync();


            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return JsonConvert.DeserializeObject<List<Coupon>>(results.Result.ToString());

            }
            return new List<Coupon>();
        }

        public async Task<Coupon> GetCoupon(string code)
        {
            var response = await _httpClient.GetAsync($"{BASEURL}/api/Coupon/{code}");
            var content = await response.Content.ReadAsStringAsync();


            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return JsonConvert.DeserializeObject<Coupon>(results.Result.ToString());

            }
            return new Coupon();
        }

        public async Task<ResponseDto> UpdateCoupon(Guid Id, AddCouponDto updCoupon)
        {
            var request = JsonConvert.SerializeObject(updCoupon);
            var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{BASEURL}/api/Coupon/{Id}", bodyContent);
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
