using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsersAPI.Helpers
{
    public class JWTHelper
    {
        public static async Task<Token> GenerateToken(string clientId, string clientSecret, string authCode)
        {
            var client = new HttpClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("code", authCode),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

            var result = await client.PostAsync("https://appleid.apple.com/auth/token", content);
            string resultContent = await result.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Token>(resultContent);
        }

        public static string GetAppleClientSecret(string teamId, string clientId, string keyId, string p8key)
        {
            string audience = "https://appleid.apple.com";

            string issuer = teamId;
            string subject = clientId;
            string kid = keyId;

            IList<Claim> claims = new List<Claim> {
                new Claim ("sub", subject)
            };

            var ecdsa = ECDsa.Create();
            ecdsa?.ImportPkcs8PrivateKey(Convert.FromBase64String(p8key), out _);

            SigningCredentials signingCred = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddDays(180),
                signingCred
            );
            token.Header.Add("kid", kid);
            token.Header.Remove("typ");

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }

    public class Token
    {
        
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
    }
}
