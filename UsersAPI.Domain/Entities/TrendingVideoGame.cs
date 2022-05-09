using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsersAPI.Domain.Entities
{
    public class TrendingVideoGame
    {
        [BsonElement("id")]
        public int VideoGameId { get; set; }
        [BsonElement("cover")]
        public string Cover { get; set; } = string.Empty;
        [BsonElement("total_now_playing")]
        public int TotalNowPlaying { get; set; }
        [BsonElement("average_star_rating")]
        public double AverageStarRating { get; set; }
        [JsonIgnore]
        [BsonElement("average_game_rating")]
        public GameRating AverageGameRating { get; set; }
        public BestGameRating BestGameRating { get
            {
                var max = AverageGameRating.GetType().GetProperties().ToDictionary(p => p.Name.ToLower(), p => (double)p.GetValue(AverageGameRating)).MaxBy(x => x.Value);
                return new BestGameRating()
                {
                    Type = max.Key,
                    Value = max.Value
                };
            }
        }
    }

    public class BestGameRating
    {
        public string Type { get; set; }
        public double Value { get; set; }
    }
}
