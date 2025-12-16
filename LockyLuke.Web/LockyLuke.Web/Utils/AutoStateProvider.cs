using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace LockyLuke.Web.Utils
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient client;
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(ILocalStorageService LocalStorageService, HttpClient Client)
        {
            localStorageService = LocalStorageService;
            client = Client;
            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string? accessToken = await localStorageService.GetItemAsStringAsync("token");

                if (string.IsNullOrEmpty(accessToken))
                    return anonymous;
                
                string? userid = await localStorageService.GetItemAsStringAsync("userid");
                var cp = new ClaimsPrincipal(new ClaimsIdentity([
               new Claim(ClaimTypes.Name, accessToken),
                new Claim(ClaimTypes.NameIdentifier, userid) ], "jwtAuthType"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                return new AuthenticationState(cp);
            }
            catch (Exception)
            {
                return anonymous;
            }
          
        }

        public async void  NotifyUserLogin(string token,string userid)
        {
           var cp =  new ClaimsPrincipal(new ClaimsIdentity([
                new Claim(ClaimTypes.Name, token),
                new Claim(ClaimTypes.NameIdentifier, userid)
            ], "jwtAuthType"));

            var authState = Task.FromResult(new AuthenticationState(cp));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}