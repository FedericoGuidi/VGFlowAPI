using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Application.Models;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Application.Business
{
    public interface IRatingService
    {
        public Task<bool> RateByStars(StarRating rating);
        public Task<bool> RateByGameRating(UserGameRating rating);
    }
}
