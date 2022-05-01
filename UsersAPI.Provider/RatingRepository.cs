using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Application.Providers;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Provider
{
    public class RatingRepository : BaseRepository, IRatingRepository
    {
        private readonly IMongoDatabase _database;
        public RatingRepository(IConfiguration config, IWebHostEnvironment env) : base(config, env)
        {
            _database = client.GetDatabase("usersDB");
        }

        public async Task<Rating> RetrieveAsync(int videoGameId, string userId)
        {
            var filterBuilder = Builders<Rating>.Filter;
            var filter = filterBuilder.Eq("user", userId) & filterBuilder.Eq("videogame", videoGameId);
            try
            {
                var rating = await _database.GetCollection<Rating>("ratings").Find(filter).FirstAsync();
                return rating;
            }
            catch (Exception)
            {
                return new Rating();
            }
        }

        public async Task<List<Rating>> RetrieveManyAsync(int videoGameId)
        {
            var filterBuilder = Builders<Rating>.Filter;
            var filter = filterBuilder.Eq("videogame", videoGameId);
            try
            {
                var ratings = await _database.GetCollection<Rating>("ratings").Find(filter).ToListAsync();
                return ratings;
            }
            catch (Exception)
            {
                return new List<Rating>();
            }
            
        }

        public async Task<bool> UpdateStarRating(int videoGameId, string userId, double stars)
        {
            var setStarRating = Builders<Rating>.Update.Set(x => x.StarRating, stars);
            var collection = _database.GetCollection<Rating>("ratings");
            try
            {
                var rating = await collection.UpdateOneAsync(x => x.VideoGame == videoGameId && x.User == userId, setStarRating);
                if (rating is not null && rating.IsModifiedCountAvailable && rating.MatchedCount > 0) return true;
                else
                {
                    // Inserimento nuovo rating
                    Rating newRating = new()
                    {
                        User = userId,
                        VideoGame = videoGameId,
                        StarRating = stars,
                        GameRating = new GameRating()
                    };
                    await collection.InsertOneAsync(newRating);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateGameRating(int videoGameId, string userId, GameRating gameRating)
        {
            var setGameRating = Builders<Rating>.Update.Set(x => x.GameRating, gameRating);
            var collection = _database.GetCollection<Rating>("ratings");
            try
            {
                var rating = await collection.UpdateOneAsync(x => x.VideoGame == videoGameId && x.User == userId, setGameRating);
                if (rating is not null && rating.IsModifiedCountAvailable && rating.MatchedCount > 0) return true;
                else
                {
                    // Inserimento nuovo rating
                    Rating newRating = new()
                    {
                        User = userId,
                        VideoGame = videoGameId,
                        StarRating = null,
                        GameRating = gameRating
                    };
                    await collection.InsertOneAsync(newRating);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
