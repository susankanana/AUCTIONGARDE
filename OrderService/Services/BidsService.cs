using Newtonsoft.Json;
using OrderService.Models.Dtos;
using OrderService.Services.IServices;
using System.Net.Http.Headers;

namespace OrderService.Services
{
    public class BidsService : IBid
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BidsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<BidDto> GetBidById(Guid Id, string token)
        {
            var client = _httpClientFactory.CreateClient("Bids");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(Id.ToString());
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"JSON Content: {content}");
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto != null && response.IsSuccessStatusCode)
            {
                var bid = JsonConvert.DeserializeObject<BidDto>(responseDto.Result.ToString());
                return bid;
            }
            return new BidDto();
        }
    }
}
