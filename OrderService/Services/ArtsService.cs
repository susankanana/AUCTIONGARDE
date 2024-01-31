using Newtonsoft.Json;
using OrderService.Models.Dtos;
using OrderService.Services.IServices;
using System.Net.Http.Headers;

namespace OrderService.Services
{
    public class ArtsService : IArt
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ArtsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ArtDto> GetArtById(Guid Id, string token)
        {
            var client = _httpClientFactory.CreateClient("Arts");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(Id.ToString());
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto.Result != null && response.IsSuccessStatusCode)
            {
                var art = JsonConvert.DeserializeObject<ArtDto>(responseDto.Result.ToString());
                return art;
            }
            return new ArtDto();
        }
    }
}
