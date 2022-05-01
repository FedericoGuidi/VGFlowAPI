using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Application.Providers
{
    public interface IRatingRepository
    {
        public Task<Rating> RetrieveAsync(int videoGameId, string userId);
        public Task<List<Rating>> RetrieveManyAsync(int videoGameId);
        public Task<bool> UpdateStarRating(int videoGameId, string userId, double stars);
        public Task<bool> UpdateGameRating(int videoGameId, string userId, GameRating gameRating);
    }
}
