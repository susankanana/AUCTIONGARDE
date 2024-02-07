using AUCTIONGARDE_Frontend.Models.Arts;
using AUCTIONGARDE_Frontend.Models.Arts.Dtos;
using AUCTIONGARDE_Frontend.Models.User.Dtos;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace AUCTIONGARDE_Frontend.Services.ArtS
{
    public class ArtService : IArt
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        //private readonly string BASEURL = "https://localhost:7223";
        private readonly string BASEURL = "https://localhost:7119";
        public ArtService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<Art>> GetAllArtsStatusTrue()
        {

            var response = await _httpClient.GetAsync($"{BASEURL}/api/Art/True");
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
        public async Task<ResponseDto> AddArt(AddArtDto art)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                var request = JsonConvert.SerializeObject(art);
                var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BASEURL}/api/Art", bodyContent);
                var content = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<ResponseDto>(content);
                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return results;
                    }
                }
            }
            return new ResponseDto();
        }

        public async Task<ResponseDto> EditArt(Art art)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                var request = JsonConvert.SerializeObject(art);
                var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{BASEURL}/api/Art", bodyContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        var results = JsonConvert.DeserializeObject<ResponseDto>(content);
                        if (results != null && results.IsSuccess)
                        {
                            if (results.Result != null)
                            {
                                return results;
                            }
                        }
                    }
                }
            }
            return new ResponseDto();
        }
    }
}
