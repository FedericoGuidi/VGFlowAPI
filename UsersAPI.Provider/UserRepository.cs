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
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly IMongoDatabase _database;
        public UserRepository(IConfiguration config, IWebHostEnvironment env) : base(config, env)
        {
            _database = client.GetDatabase("usersDB");
        }

        public async Task<User> RetrieveAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq("apple_id", id);
            var user = await _database.GetCollection<User>("users").Find(filter).FirstAsync();
            return user;
        }

        public async Task<bool> InsertUpdateVideoGame(string userId, VideoGame videoGame)
        {
            try
            {
                var collection = _database.GetCollection<User>("users");
                var filter = Builders<User>.Filter.Eq(x => x.AppleId, userId)
                    & Builders<User>.Filter.ElemMatch(x => x.VideoGames, Builders<VideoGame>.Filter.Eq(x => x.Id, videoGame.Id));

                var update = Builders<User>.Update.Set(x => x.VideoGames![-1].Hours, videoGame.Hours)
                    .Set(x => x.VideoGames![-1].Status, videoGame.Status)
                    .Set(x => x.VideoGames![-1].Starred, videoGame.Starred)
                    .Set(x => x.VideoGames![-1].NowPlaying, videoGame.NowPlaying)
                    .Set(x => x.VideoGames![-1].UpdatedAt, DateTime.Now);

                var result = await collection.UpdateOneAsync(filter, update);

                if (result is not null && result.IsModifiedCountAvailable && result.MatchedCount > 0) return true;
                else
                {
                    videoGame.AddedAt = DateTime.Now;
                    videoGame.UpdatedAt = DateTime.Now;

                    var insertFilter = Builders<User>.Filter.Where(x => x.AppleId == userId);
                    var insertUpdate = Builders<User>.Update.Push("videogames", videoGame);
                    await collection.FindOneAndUpdateAsync(insertFilter, insertUpdate);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteVideoGame(int id, string userId)
        {
            try
            {
                var collection = _database.GetCollection<User>("users");
                var filter = Builders<User>.Filter.Where(x => x.AppleId == userId);
                var update = Builders<User>.Update.PullFilter(x => x.VideoGames, Builders<VideoGame>.Filter.Where(v => v.Id == id));
                await collection.UpdateOneAsync(filter, update);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Rating> RetrieveAverageRating(int videoGameId)
        {
            PipelineDefinition<User, Rating> pipeline = new[]
            {
                new BsonDocument("$unwind",
                new BsonDocument("path", "$videogames")),
                new BsonDocument("$match",
                new BsonDocument("videogames.id", videoGameId)),
                new BsonDocument("$group",
                new BsonDocument
                    {
                        { "_id", "$videogames.id" },
                        { "star_rating",
                new BsonDocument("$avg", "$videogames.star_rating") },
                        { "gameplay",
                new BsonDocument("$avg", "$videogames.game_rating.gameplay") },
                        { "plot",
                new BsonDocument("$avg", "$videogames.game_rating.plot") },
                        { "music",
                new BsonDocument("$avg", "$videogames.game_rating.music") },
                        { "graphics",
                new BsonDocument("$avg", "$videogames.game_rating.graphics") },
                        { "level_design",
                new BsonDocument("$avg", "$videogames.game_rating.level_design") },
                        { "longevity",
                new BsonDocument("$avg", "$videogames.game_rating.longevity") },
                        { "ia",
                new BsonDocument("$avg", "$videogames.game_rating.ia") },
                        { "physics",
                new BsonDocument("$avg", "$videogames.game_rating.physics") }
                    }),
                new BsonDocument("$project",
                new BsonDocument
                    {
                        { "_id", 0 },
                        { "videogame", "$_id" },
                        { "star_rating", "$star_rating" },
                        { "game_rating",
                new BsonDocument
                        {
                            { "gameplay", "$gameplay" },
                            { "plot", "$plot" },
                            { "music", "$music" },
                            { "graphics", "$graphics" },
                            { "level_design", "$level_design" },
                            { "longevity", "$longevity" },
                            { "ia", "$ia" },
                            { "physics", "$physics" }
                        } }
                    })
            };

            var cursor = await _database.GetCollection<User>("users").AggregateAsync(pipeline);
            var listResult = await cursor.ToListAsync();
            
            return listResult.Any() ? listResult.First() : new Rating();
        }

        public async Task<bool> UpdateStarRating(int videoGameId, string userId, double stars)
        {
            try
            {
                var collection = _database.GetCollection<User>("users");
                var filter = Builders<User>.Filter.Eq(x => x.AppleId, userId)
                    & Builders<User>.Filter.ElemMatch(x => x.VideoGames, Builders<VideoGame>.Filter.Eq(x => x.Id, videoGameId));

                var update = Builders<User>.Update.Set(x => x.VideoGames![-1].StarRating, stars);

                var result = await collection.UpdateOneAsync(filter, update);
                if (result is not null && result.IsModifiedCountAvailable && result.MatchedCount > 0) return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateGameRating(int videoGameId, string userId, GameRating gameRating)
        {
            try
            {
                var collection = _database.GetCollection<User>("users");
                var filter = Builders<User>.Filter.Eq(x => x.AppleId, userId)
                    & Builders<User>.Filter.ElemMatch(x => x.VideoGames, Builders<VideoGame>.Filter.Eq(x => x.Id, videoGameId));

                var update = Builders<User>.Update.Set(x => x.VideoGames![-1].GameRating, gameRating);

                var result = await collection.UpdateOneAsync(filter, update);
                if (result is not null && result.IsModifiedCountAvailable && result.MatchedCount > 0) return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<TrendingVideoGame>> RetrieveTrendingVideoGames()
        {
            PipelineDefinition<User, TrendingVideoGame> pipeline = new[]
            {
                new BsonDocument("$unwind",
                new BsonDocument("path", "$videogames")),
                new BsonDocument("$group",
                new BsonDocument
                    {
                        { "_id", "$videogames.id" },
                        { "cover",
                new BsonDocument("$first", "$videogames.cover") },
                        { "average_star_rating",
                new BsonDocument("$avg", "$videogames.star_rating") },
                        { "total_now_playing",
                new BsonDocument("$sum",
                new BsonDocument("$cond",
                new BsonArray
                                {
                                    new BsonDocument("$eq",
                                    new BsonArray
                                        {
                                            "$videogames.now_playing",
                                            true
                                        }),
                                    1,
                                    0
                                })) },
                        { "gameplay",
                new BsonDocument("$avg", "$videogames.game_rating.gameplay") },
                        { "plot",
                new BsonDocument("$avg", "$videogames.game_rating.plot") },
                        { "music",
                new BsonDocument("$avg", "$videogames.game_rating.music") },
                        { "graphics",
                new BsonDocument("$avg", "$videogames.game_rating.graphics") },
                        { "level_design",
                new BsonDocument("$avg", "$videogames.game_rating.level_design") },
                        { "longevity",
                new BsonDocument("$avg", "$videogames.game_rating.longevity") },
                        { "ia",
                new BsonDocument("$avg", "$videogames.game_rating.ia") },
                        { "physics",
                new BsonDocument("$avg", "$videogames.game_rating.physics") }
                    }),
                new BsonDocument("$match",
                new BsonDocument
                    {
                        { "total_now_playing",
                new BsonDocument("$gt", 0) },
                        { "average_star_rating",
                new BsonDocument("$ne", BsonNull.Value) },
                        { "gameplay",
                new BsonDocument("$ne", BsonNull.Value) }
                    }),
                new BsonDocument("$project",
                new BsonDocument
                    {
                        { "_id", 0 },
                        { "id", "$_id" },
                        { "cover", "$cover" },
                        { "total_now_playing", "$total_now_playing" },
                        { "average_star_rating", "$average_star_rating" },
                        { "average_game_rating",
                new BsonDocument
                        {
                            { "gameplay", "$gameplay" },
                            { "plot", "$plot" },
                            { "music", "$music" },
                            { "graphics", "$graphics" },
                            { "level_design", "$level_design" },
                            { "longevity", "$longevity" },
                            { "ia", "$ia" },
                            { "physics", "$physics" }
                        } }
                    }),
                new BsonDocument("$sort",
                new BsonDocument("total_now_playing", -1)),
                new BsonDocument("$limit", 9)
            };

            var cursor = await _database.GetCollection<User>("users").AggregateAsync(pipeline);
            return await cursor.ToListAsync();
        }
    }
}
