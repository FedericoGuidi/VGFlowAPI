﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Application.Models;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Application.Business
{
    public interface IUserService
    {
        public Task<User> RetrieveAsync(string id);
        public Task<Profile> RetrieveProfileAsync(string id);
        public Task<VideoGameDetails> RetrieveVideoGameDetailsAsync(int videoGameId, string userId);
        public Task<bool> InsertUpdateVideoGame(BacklogEntry backlogEntry);
    }
}