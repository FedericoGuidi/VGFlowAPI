using System.Text.Json.Serialization;

namespace VideoGamesAPI.Business.Models
{
    public class Screenshot
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("image_id")]
        public string ImageId { get; set; } = string.Empty;
        [JsonPropertyName("screenshot_url")]
        public Uri ScreenshotUrl
        {
            get
            {
                string url = "https://images.igdb.com/igdb/image/upload/t_screenshot_big/{0}.png";
                return new Uri(string.Format(url, ImageId));
            }
        }
    }
}