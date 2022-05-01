using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VideoGamesAPI.Business.Models
{
    public class Cover
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("image_id")]
        public string ImageId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("size")]
        public ImageSize Size { get; set; }
        [JsonPropertyName("image_url")]
        public Uri ImageUrl
        {
            get
            {
                string url = "https://images.igdb.com/igdb/image/upload/t_{0}_2x/{1}.png";
                string size = string.Empty;
                switch (Size)
                {
                    case ImageSize.Thumbnail:
                        size = "thumb";
                        break;
                    case ImageSize.Small:
                        size = "cover_small";
                        break;
                    case ImageSize.Big:
                        size = "cover_big";
                        break;
                }
                return new Uri(string.Format(url, size, ImageId));
            }
        }
    }

    public enum ImageSize
    {
        Thumbnail,
        Small,
        Big
    }
}
