using AUCTIONGARDE_Frontend.Models.Order;
using AUCTIONGARDE_Frontend.Models.Order.Dtos;
using AUCTIONGARDE_Frontend.Models.User.Dtos;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace AUCTIONGARDE_Frontend.Services.OrderS
{
    public class OrderService : IOrder
    {
        private readonly HttpClient _httpClient;
        //private readonly string BASEURL = "https://localhost:7171"; 
        private readonly string BASEURL = "https://localhost:7119";
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigation;
        public OrderService(HttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigation)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _navigation = navigation;
        }
        public async Task<ResponseDto> MakeOrder(AddOrderDto newOrder)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var request = JsonConvert.SerializeObject(newOrder);
                var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");
                //communicate wih Api

                var response = await _httpClient.PostAsync($"{BASEURL}/api/Order", bodyContent);
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
        public async Task<Order> GetOrderById(Guid orderId)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"{BASEURL}/api/Order/{orderId}");
                var content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return JsonConvert.DeserializeObject<Order>(results.Result.ToString());
                    }
                }
            }
            return new Order();
        }

        public async Task<StripeRequestDto> MakePayments(StripeRequestDto stripe)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = JsonConvert.SerializeObject(stripe);
                var bodyContent = new StringContent(request, Encoding.UTF8, "application/json");
                //communicate wih Api

                var response = await _httpClient.PostAsync($"{BASEURL}/api/Order/Pay", bodyContent);
                var content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        //return JsonConvert.DeserializeObject<StripeRequestDto>(results.Result.ToString());
                        var stripeResponseDto = JsonConvert.DeserializeObject<StripeRequestDto>(results.Result.ToString());

                        // Access the approved URL from the StripeResponseDto
                        var stripeSessionUrl = stripeResponseDto.StripeSessionUrl;

                        // Navigate the user to the approved URL
                        _navigation.NavigateTo(stripeSessionUrl);
                    }
                }
            }
            return new StripeRequestDto();
        }

        public async Task<bool> ValidatePayment(Guid orderId)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsync($"{BASEURL}/api/Order/validate/{orderId}", null);
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

        public async Task<List<Order>> GetOrdersByUserId(Guid userId)
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"{BASEURL}/api/Order/User/{userId}");
                var content = await response.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<ResponseDto>(content);

                if (results != null && results.IsSuccess)
                {
                    if (results.Result != null)
                    {
                        return JsonConvert.DeserializeObject<List<Order>>(results.Result.ToString());
                    }
                }
            }
            return new List<Order>();
        }
    }
}
