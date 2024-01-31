using AUCTIONGARDE_Frontend.Models.User;
using AUCTIONGARDE_Frontend.Models.User.Dtos;
using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace AUCTIONGARDE_Frontend.Services.Auth
{
    public class UserService : IUser
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly string BASEURL = "https://localhost:7000";
        public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public async Task<User> GetUserById(string id)
        {
            var response = await _httpClient.GetAsync($"{BASEURL}/api/User/{id}");
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results.IsSuccess)
            {
                //change this to a list of products
                return JsonConvert.DeserializeObject<User>(results.Result.ToString());

            }
            return new User();
        }
    }
}
