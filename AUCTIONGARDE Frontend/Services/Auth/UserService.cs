using AUCTIONGARDE_Frontend.Models.User;
using AUCTIONGARDE_Frontend.Models.User.Dtos;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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

        public async Task<List<User>> GetAllUsers()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"{BASEURL}/api/User");
                var content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return JsonConvert.DeserializeObject<List<User>>(results.Result.ToString());
                    }
                }
            }
            return new List<User>();
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
        public async Task<bool> RemoveUser(string userId)
        {

            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"{BASEURL}/api/User/{userId}");
                var content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
