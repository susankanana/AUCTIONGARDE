using AUCTIONGARDE_Frontend.Models.User;
using AUCTIONGARDE_Frontend.Models.User.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace AUCTIONGARDE_Frontend.Services.Auth
{
    public class AuthServiceLogin : IAuthLogin
    {
        private readonly HttpClient _httpClient;
        private readonly string BASEURL = "https://localhost:7000";
        public AuthServiceLogin(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponseDto> Login(LoginUser loginRequestDto)
        {
            var request = JsonConvert.SerializeObject(loginRequestDto);
            var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");
            //communicate wih Api

            var response = await _httpClient.PostAsync($"{BASEURL}/api/User/Login", bodyContent);
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                var res = JsonConvert.DeserializeObject<LoginResponseDto>(results.Result.ToString());
                if(res!=null) {
                    return res;
                }
                
            }
            return new LoginResponseDto();
        }

    }
}
