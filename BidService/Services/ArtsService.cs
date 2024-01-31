using BidService.Models.Dtos;
using BidService.Services.Iservices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BidService.Services
{
    public class ArtsService : IArt
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient client;

        public ArtsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            client = _httpClientFactory.CreateClient("Arts");
        }
        public async Task<ArtDto> GetArtById(Guid Id, string Token)
        {
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.GetAsync(Id.ToString());
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto.Result != null && response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ArtDto>(responseDto.Result.ToString());
            }
            return new ArtDto();
        }

        public async Task<string> UpdateArtHighestBid(Guid artId, int highestBid)
        {

          // var updateData = new { ArtId = artId, HighestBid = highestBid };
           // var requestContent = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

            await client.GetAsync($"UpdateHighestBid/{artId}/{highestBid}");

            return "Updated successfully";

        }
    }
}
