using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using VideoGamesAPI.Business.Models;

namespace VideoGamesAPI.Business
{
    public class IGDBAPI
    {
        public HttpClient HttpClient;
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment Environment;
        private readonly string ClientID;
        private readonly string ClientSecret;

        public IGDBAPI(IConfiguration config, IWebHostEnvironment environment)
        {
            Configuration = config;
            Environment = environment;

            if (Environment.IsDevelopment())
            {
                ClientID = Configuration["IGDB:ClientID"];
                ClientSecret = Configuration["IGDB:ClientSecret"];
            }
            else
            {
                var client = new SecretClient(new Uri(uriString: $"https://vgflow.vault.azure.net/"), new DefaultAzureCredential());
                var clientIDsecret = client.GetSecretAsync("IGDBClientID").Result;
                var clientSecretSecret = client.GetSecretAsync("IGDBClientSecret").Result;
                ClientID = clientIDsecret.Value.Value.ToString();
                ClientSecret = clientSecretSecret.Value.Value.ToString();
            }

            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            HttpClient = new HttpClient(clientHandler);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorize().Result);
            HttpClient.DefaultRequestHeaders.Add("Client-ID", ClientID);
        }

        public async Task<string> Authorize()
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            HttpClient client = new(clientHandler);
            var values = new Dictionary<string, string>
            {
                { "client_id", ClientID },
                { "client_secret", ClientSecret },
                { "grant_type", "client_credentials" }
            };

            string url = "https://id.twitch.tv/oauth2/token";
            var data = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, data);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var bearer = JsonSerializer.Deserialize<IGDBToken>(jsonResponse);
            if (bearer != null) { return bearer.AccessToken; }
            return string.Empty;
        }
    }
}