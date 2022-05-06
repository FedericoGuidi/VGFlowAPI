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
    }
}
