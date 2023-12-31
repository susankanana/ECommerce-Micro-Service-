﻿using Newtonsoft.Json;
using OrderService.Models.Dtos;
using OrderService.Services.IServices;

namespace OrderService.Services
{
    public class UserService : IUser
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<UserDto> GetUserById(string Id)
        {
            var client = _httpClientFactory.CreateClient("Users");
            var response = await client.GetAsync(Id);
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto.Result != null && response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<UserDto>(responseDto.Result.ToString());
            }
            return new UserDto();
        }
    }
    
}
