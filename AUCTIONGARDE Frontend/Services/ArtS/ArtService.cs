using AUCTIONGARDE_Frontend.Models.Arts;
using AUCTIONGARDE_Frontend.Models.User.Dtos;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace AUCTIONGARDE_Frontend.Services.ArtS
{
    public class ArtService : IArt
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly string BASEURL = "https://localhost:7223";
        public ArtService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<Art>> GetAllArts()
        {

            var response = await _httpClient.GetAsync($"{BASEURL}/api/Art");
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<ResponseDto>(content);

            if (results != null && results.IsSuccess)
            {
                if (results.Result != null)
                {
                    return JsonConvert.DeserializeObject<List<Art>>(results.Result.ToString());
                }
            }

            return new List<Art>();
        }

        public async Task<List<Art>> GetArtsByUserId()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"{BASEURL}/api/Art/Seller");
                var content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return JsonConvert.DeserializeObject<List<Art>>(results.Result.ToString());
                    }
                }
            }
            return new List<Art>();
        }

        public async Task<bool> DeleteArt(Guid artId)
        {

            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"{BASEURL}/api/Art/{artId}");
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
