using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Domain.Entities
{
    public class Rating
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("videogame")]
        public int VideoGame { get; set; }
        [BsonElement("star_rating")]
        public double? StarRating { get; set; }
        [BsonElement("game_rating")]
        public GameRating GameRating { get; set; }
        [BsonElement("star_rating_count")]
        public int StarRatingCount { get; set; }
        [BsonElement("game_rating_count")]
        public int GameRatingCount { get; set; }
    }

    public class GameRating
    {
        public GameRating()
        {
            Gameplay = 0;
            Plot = 0;
            Music = 0;
            Graphics = 0;
            LevelDesign = 0;
            Longevity = 0;
            IA = 0;
            Physics = 0;
        }

        [BsonElement("gameplay")]
        public double? Gameplay { get; set; }
        [BsonElement("plot")]
        public double? Plot { get; set; }
        [BsonElement("music")]
        public double? Music { get; set; }
        [BsonElement("graphics")]
        public double? Graphics { get; set; }
        [BsonElement("level_design")]
        public double? LevelDesign { get; set; }
        [BsonElement("longevity")]
        public double? Longevity { get; set; }
        [BsonElement("ia")]
        public double? IA { get; set; }
        [BsonElement("physics")]
        public double? Physics { get; set; }
    }
}
