using UsersAPI.Application.Business;
using UsersAPI.Application.Models;
using UsersAPI.Application.Providers;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Business
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        public RatingService(IRatingRepository ratingRepository)
        {

            _ratingRepository = ratingRepository;
        }

        public async Task<bool> RateByStars(StarRating rating)
        {
            return await _ratingRepository.UpdateStarRating(rating.VideoGameId, rating.User, rating.Stars);
        }

        public async Task<bool> RateByGameRating(UserGameRating rating)
        {
            GameRating gameRating = new()
            {
                Gameplay = rating.Gameplay,
                Plot = rating.Plot,
                Music = rating.Music,
                Graphics = rating.Graphics,
                LevelDesign = rating.LevelDesign,
                Longevity = rating.Longevity,
                IA = rating.IA,
                Physics = rating.Physics
            };

            return await _ratingRepository.UpdateGameRating(rating.VideoGameId, rating.User, gameRating);
        }
    }
}