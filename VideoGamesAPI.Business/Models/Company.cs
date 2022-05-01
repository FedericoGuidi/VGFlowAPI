using System.Text.Json.Serialization;

namespace VideoGamesAPI.Business.Models
{
    public class Company
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class InvolvedCompany
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("company")]
        public Company Company { get; set; }
        [JsonPropertyName("developer")]
        public bool Developer { get; set; }
        [JsonPropertyName("publisher")]
        public bool Publisher { get; set; }
        [JsonPropertyName("supporting")]
        public bool Supporting { get; set; }
    }
}