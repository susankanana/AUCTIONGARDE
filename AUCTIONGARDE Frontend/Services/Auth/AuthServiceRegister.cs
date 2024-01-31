using AUCTIONGARDE_Frontend.Models.User;
using AUCTIONGARDE_Frontend.Models.User.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace AUCTIONGARDE_Frontend.Services.Auth
{
    public class AuthServiceRegister : IAuthRegister
    {
        private readonly HttpClient _httpClient;
        private readonly string BASEURL = "https://localhost:7000";

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
