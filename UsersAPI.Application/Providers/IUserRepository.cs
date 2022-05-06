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
        public Task<bool> InsertUpdateVideoGame(string userId, VideoGame videoGame);
        public Task<bool> DeleteVideoGame(int id, string userId);
    }
}
