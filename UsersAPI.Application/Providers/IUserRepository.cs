using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Application.Providers
{
    public interface IUserRepository
    {
        public Task<User> RetrieveAsync(string id);
        public Task InsertOneAsync(User user);
        public Task<bool> UpdateOneAsync(User user);

        public Task<bool> InsertUpdateVideoGame(string userId, VideoGame videoGame);
        public Task<bool> DeleteVideoGame(int id, string userId);

        public Task<Rating> RetrieveAverageRating(int videoGameId);
        public Task<bool> UpdateStarRating(int videoGameId, string userId, double stars);
        public Task<bool> UpdateGameRating(int videoGameId, string userId, GameRating gameRating);

        public Task<IEnumerable<TrendingVideoGame>> RetrieveTrendingVideoGames();
    }
}
