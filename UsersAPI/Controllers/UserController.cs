using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Application.Business;
using UsersAPI.Application.Models;
using UsersAPI.Domain.Entities;
using UsersAPI.Helpers;

namespace UsersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly string TeamID;
        private readonly string ClientID;
        private readonly string KeyID;
        private readonly string SignatureKey;

        public UserController(IUserService userService, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _userService = userService;
            _configuration = configuration;
            _environment = environment;

            if (_environment.IsDevelopment())
            {
                TeamID = _configuration["AppleID:TeamID"];
                ClientID = _configuration["AppleID:ClientID"];
                KeyID = _configuration["AppleID:KeyID"];
                SignatureKey = _configuration["AppleID:KeyID"];
            }
            else
            {
                var client = new SecretClient(new Uri(uriString: $"https://vgflow.vault.azure.net/"), new DefaultAzureCredential());
                var teamIDsecret = client.GetSecretAsync("AppleTeamID").Result;
                var clientIDsecret = client.GetSecretAsync("AppleClientID").Result;
                var keyIDsecret = client.GetSecretAsync("AppleKeyID").Result;
                var signatureKeySecret = client.GetSecretAsync("AppleSignatureKey").Result;
                TeamID = teamIDsecret.Value.Value.ToString();
                ClientID = clientIDsecret.Value.Value.ToString();
                KeyID = keyIDsecret.Value.Value.ToString();
                SignatureKey = signatureKeySecret.Value.Value.ToString();
            }
        }

        [HttpGet]
        public async Task<ActionResult<User>> Get(string id)
        {
            User user = await _userService.RetrieveAsync(id);
            return Ok(user);
        }

        [HttpGet]
        [Route("profile")]
        public async Task<ActionResult<Profile>> GetProfile(string id)
        {
            Profile profile = await _userService.RetrieveProfileAsync(id);
            return Ok(profile);
        }

        [HttpGet]
        [Route("videogame/details")]
        public async Task<ActionResult<VideoGameDetails>> GetVideogameDetails(int vid, string uid)
        {
            VideoGameDetails videogameDetails = await _userService.RetrieveVideoGameDetailsAsync(vid, uid);
            return Ok(videogameDetails);
        }

        [HttpPost]
        [Route("videogame/rateByStars")]
        public async Task<ActionResult> RateVideogameByStars(StarRating rating)
        {
            await _userService.RateByStars(rating);
            return NoContent();
        }

        [HttpPost]
        [Route("videogame/rateByGameRating")]
        public async Task<ActionResult> RateVideogameByGameRating(UserGameRating rating)
        {
            await _userService.RateByGameRating(rating);
            return NoContent();
        }

        [HttpPost]
        [Route("videogame/entry")]
        public async Task<ActionResult> VideoGameEntry(BacklogEntry backlog)
        {
            await _userService.InsertUpdateVideoGame(backlog);
            return NoContent();
        }

        [HttpDelete]
        [Route("videogame/removeentry")]
        public async Task<ActionResult> RemoveVideoGameEntry(int id, string userId)
        {
            await _userService.DeleteVideoGame(id, userId);
            return NoContent();
        }

        [HttpGet]
        [Route("videogames/trending")]
        public async Task<ActionResult<IEnumerable<TrendingVideoGame>>> GetTrendingVideoGames()
        {
            IEnumerable<TrendingVideoGame> trendingVideoGames = await _userService.RetrieveTrendingVideoGames();
            return Ok(trendingVideoGames);
        }

        [HttpPost]
        [Route("login")]
        public async Task<Token> Login(string ac)
        {
            var clientSecret = JWTHelper.GetAppleClientSecret(TeamID, ClientID, KeyID, SignatureKey);
            return await JWTHelper.GenerateToken(ClientID, clientSecret, ac);
        }
    }
}
