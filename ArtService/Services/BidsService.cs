using ArtService.Services.IServices;
using System.Text;
using System.Text.Json;

namespace ArtService.Services
{
    public class BidsService : IBid
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7249/api/Bid/update/bidIds";
        public BidsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> UpdateBidStatus(List<string> artIds)
        {
            string jsoncontent = JsonSerializer.Serialize(artIds);
            HttpContent content = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiUrl, content);
            return "updated";
        }
    }
}
