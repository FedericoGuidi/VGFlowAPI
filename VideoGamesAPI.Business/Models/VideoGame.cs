using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VideoGamesAPI.Business.Models
{
    public class VideoGameLite
    {
        [JsonPropertyOrder(-2)]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyOrder(-2)]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyOrder(-2)]
        [JsonPropertyName("cover")]
        public Cover Cover
        {
            get
            {
                return _Cover;
            }
            set
            {
                _Cover = value;
                if (this is VideoGame) { _Cover.Size = ImageSize.Big; }
                else { _Cover.Size = ImageSize.Small; }
            }
        }
        private Cover _Cover;

        [JsonPropertyName("first_release_date")]
        public int UnixReleaseDate { get; set; }
        [JsonPropertyOrder(-2)]
        [JsonPropertyName("release_date")]
        public DateTime ReleaseDate
        {
            get
            {
                DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(UnixReleaseDate).ToLocalTime();
                return dateTime;
            }
        }
        [JsonPropertyOrder(-2)]
        [JsonPropertyName("platforms")]
        public List<PlatformLite> Platforms { get; set; }
    }

    public class VideoGame : VideoGameLite
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;
        [JsonPropertyName("storyline")]
        public string Storyline { get; set; } = string.Empty;
        [JsonPropertyName("involved_companies")]
        public List<InvolvedCompany> InvolvedCompanies { get; set; }
        [JsonPropertyName("age_ratings")]
        public List<AgeRating> AgeRatings { get; set; }
        [JsonPropertyName("screenshots")]
        public List<Screenshot> Screenshots { get; set; }
        [JsonPropertyName("platforms")]
        public new List<Platform> Platforms { get; set; }
    }
}
