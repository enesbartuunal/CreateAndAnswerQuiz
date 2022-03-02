using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

using CreateAndAnswerQuiz.Models.Models;
using CreateAndAnswerQuiz.UI.AuthProvider;
using CreateAndAnswerQuiz.UI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.UI.Services.Implementation
{
    public class AuthenticationHttpService : IAuthenticationHttpService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationHttpService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<string> Login(SignInModel model)
        {
            var content = JsonConvert.SerializeObject(model);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var authResult = await _client.PostAsync("account/signin", bodyContent);
            var authContent = await authResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignInResponseModel>(authContent);
            if (!authResult.IsSuccessStatusCode)
                return "InvalidAuthanticaon" ;

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            return "Welcome";   
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<IEnumerable<string>> RegisterUser(SignUpModel model)
        {
            var content = JsonConvert.SerializeObject(model);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var registrationResult = await _client.PostAsync("account/signup", bodyContent);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();
            if (!registrationResult.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<IEnumerable<string>>(registrationContent);
                return result;
            }
            return null;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            var tokenDto = JsonConvert.SerializeObject(new RefreshTokenDto { Token = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");
            var refreshResult = await _client.PostAsync("token/refresh", bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignInResponseModel>(refreshContent);
            if (!refreshResult.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong during the refresh token action");
            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            return result.Token;
        }

        public async Task<string> GetUserIdbyName(string userName)
        {
            var content = JsonConvert.SerializeObject(userName);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var registrationResult = await _client.PostAsync("account/getuserid", bodyContent);
            var registrationContent = await registrationResult.Content.ReadAsStringAsync();
            if (registrationResult.IsSuccessStatusCode)
            {

                return registrationContent;
            }
            return "User Not Found";
        }
    }
}
