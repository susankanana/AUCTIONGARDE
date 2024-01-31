using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace AUCTIONGARDE_Frontend.Services.AuthProv
{
    public class AuthProvider : AuthenticationStateProvider
    {

        private ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        public AuthProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt) //receives a token
        {
            var claims = new List<Claim>(); // we start by creating an empty list of claims
            var payload = jwt.Split('.')[1]; //splits token into 3 parts based on . and gets the middle part i.e payload

            var jsonBytes = ParseBase64WithoutPadding(payload); //a  a and b are trying to extract what data is encoded in the token and

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);  //b list of claims as keyvalue pairs
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));  // we then add extracted data to list of claims
            return claims;  //return a list of claims containing data encoded in token
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            
            string authToken = await _localStorage.GetItemAsStringAsync("authToken");  //read token

            var identity = new ClaimsIdentity();

            _httpClient.DefaultRequestHeaders.Authorization = null;  //null atm since we are yet to confirm that there's a token
            if (!string.IsNullOrEmpty(authToken))
            {
                //we need to pass this token with every request we make
                try  
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");   //we'll get a list of claims from the method passclaimsfromjwt
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);  //.This is an interceptor. Passes the token to the header of every request go in out 
                }
                catch (Exception ex)
                {
                    await _localStorage.RemoveItemAsync("authToken"); //remove token because we have an error
                    identity = new ClaimsIdentity(); //reset it
                }
            }
            //we need to notify blazor that state has changed i.e we are now logged in. Determined by whether we have a token or not
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user); //reps state we are in

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;

        }
    }
}
