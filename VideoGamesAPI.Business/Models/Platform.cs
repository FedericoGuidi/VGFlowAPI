using System.Text.Json.Serialization;

namespace VideoGamesAPI.Business.Models
{
    public class PlatformLite
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; } = string.Empty;
    }

    public class Platform : PlatformLite
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("platform_logo")]
        public PlatformLogo PlatformLogo { get; set; }
    }

    public class PlatformLogo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("image_id")]
        public string ImageId { get; set; }
        [JsonPropertyName("url")]
        public Uri Url
        {
            get
            {
                string url = "https://images.igdb.com/igdb/image/upload/t_cover_small_2x/{0}.png";
                return new Uri(string.Format(url, ImageId));
            }
        }
    }
}
