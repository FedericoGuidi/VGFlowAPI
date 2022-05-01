using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using VideoGamesAPI.Business.Models;

namespace VideoGamesAPI.Business
{
    public class VideoGamesService : IGDBAPI
    {
        public VideoGamesService(IConfiguration config, IWebHostEnvironment env) : base(config, env)
        {
        }

        public async Task<IEnumerable<VideoGameLite>> GetVideoGames(string input)
        {
            string search = string.IsNullOrEmpty(input) ? string.Empty : $"search \"{input}\";";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.igdb.com/v4/games"),
                Content = new StringContent("fields id,name,cover.image_id,first_release_date,platforms.abbreviation;" +
                    "limit 50;"
                    + search, Encoding.UTF8, MediaTypeNames.Application.Json),
            };

            var response = await HttpClient.SendAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<VideoGameLite>>(jsonResponse);
        }

        public async Task<VideoGame> GetVideoGameById(int id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.igdb.com/v4/games"),
                Content = new StringContent($"fields id," +
                    $"name,first_release_date,cover.image_id,summary,storyline,screenshots.*,platforms.*,platforms.platform_logo.*," +
                    $"involved_companies.company.name," +
                    $"involved_companies.publisher," +
                    $"involved_companies.developer," +
                    $"involved_companies.supporting," +
                    $"age_ratings.category," +
                    $"age_ratings.rating; where id = {id};", Encoding.UTF8, MediaTypeNames.Application.Json),
            };

            var response = await HttpClient.SendAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<VideoGame>>(jsonResponse).FirstOrDefault();
        }
    }
}
