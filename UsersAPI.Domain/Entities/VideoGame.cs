using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Domain.Entities
{
    [BsonNoId]
    public class VideoGame
    {
        [BsonElement("id")]
        public int Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("cover")]
        public string Cover { get; set; } = string.Empty;
        [BsonElement("genres")]
        public List<string>? Genres { get; set; }
        [BsonElement("hours")]
        public int Hours { get; set; }
        [BsonElement("starred")]
        public bool Starred { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement("status")]
        public Status Status { get; set; }
        [BsonElement("now_playing")]
        public bool NowPlaying { get; set; }
        [BsonElement("star_rating")]
        public double? StarRating { get; set; }
        [BsonElement("game_rating")]
        public GameRating? GameRating { get; set; }
        [BsonElement("added_at")]
        public DateTime AddedAt { get; set; }
        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class VideoGameCard
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
    }

    public enum Status
    {
        NotStarted,
        Unfinished,
        Finished,
        Completed,
        Unlimited,
        Abandoned
    }
}
