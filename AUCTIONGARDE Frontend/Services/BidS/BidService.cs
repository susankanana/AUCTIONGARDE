using AUCTIONGARDE_Frontend.Models.Bid;
using AUCTIONGARDE_Frontend.Models.Bid.Dtos;
using AUCTIONGARDE_Frontend.Models.User.Dtos;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AUCTIONGARDE_Frontend.Services.BidS
{
    public class BidService : IBid
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly string BASEURL = "https://localhost:7249";
        public BidService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<Bid>> GetBidsByUserId()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"{BASEURL}/api/Bid/Bidder");
                var content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return JsonConvert.DeserializeObject<List<Bid>>(results.Result.ToString());
                    }
                }
            }
            return new List<Bid>();
        }

        public async Task<List<Bid>> GetAllBids()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"{BASEURL}/api/Bid");
                var content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return JsonConvert.DeserializeObject<List<Bid>>(results.Result.ToString());
                    }
                }
            }
            return new List<Bid>();
        }

        public async Task<List<Bid>> GetBidsByArtId(Guid artId)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"{BASEURL}/api/Bid/{artId}");
                var content = await response.Content.ReadAsStringAsync();


                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return JsonConvert.DeserializeObject<List<Bid>>(results.Result.ToString());
                    }
                }
            }
            return new List<Bid>();
        }

        public async Task<ResponseDto> PlaceBid(AddBidDto newBid)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var request = JsonConvert.SerializeObject(newBid);
                var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");
                //communicate wih Api

                var response = await _httpClient.PutAsync($"{BASEURL}/api/Bid", bodyContent);
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

        public async Task<bool> DeleteBid(Bid bid)
        {

            if (bid.bidId == Guid.Empty)
            {
                // Handle the case where the bid doesn't have a valid BidId
                return false;
            }

            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var serializedBid = JsonConvert.SerializeObject(bid);
                var content = new StringContent(serializedBid, Encoding.UTF8, "application/json");

                var response = await _httpClient.DeleteAsync($"{BASEURL}/api/Bid");
                response.Content = content;

                var results = JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());

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
