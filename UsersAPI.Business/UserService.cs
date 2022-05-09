﻿using UsersAPI.Application.Business;
using UsersAPI.Application.Models;
using UsersAPI.Application.Providers;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RetrieveAsync(string id)
        {
            return await _userRepository.RetrieveAsync(id);
        }

        public async Task<Profile> RetrieveProfileAsync(string id)
        {
            User user = await _userRepository.RetrieveAsync(id);
            Profile profile = new Profile();

            profile.Name = user.Name;
            profile.Description = user.Description;
            profile.Social = user.Social;

            Backlog backlog = new();
            backlog.TotalHours = user.VideoGames?.Sum(x => x.Hours) ?? 0;
            backlog.TotalGames = user.VideoGames?.Count ?? 0;
            backlog.MostPlayedGenre = user.VideoGames?.SelectMany(x => x.Genres ?? new List<string>())
                .GroupBy(x => x)
                .MaxBy(x => x.Count())
                .First();

            profile.Backlog = backlog;

            profile.NowPlaying = user.VideoGames?.Where(x => x.NowPlaying).OrderByDescending(x => x.UpdatedAt).Select(x => MapFrom(x)).ToList();
            profile.Favorites = user.VideoGames?.Where(x => x.Starred).OrderByDescending(x => x.UpdatedAt).Select(x => MapFrom(x)).ToList();

            return profile;
        }

        public async Task<VideoGameDetails> RetrieveVideoGameDetailsAsync(int videogameId, string userId)
        {
            // Dettagli relativi all'utente
            User user = await _userRepository.RetrieveAsync(userId);

            Rating averageRating = await _userRepository.RetrieveAverageRating(videogameId);
           
            var videogame = user.VideoGames?.FirstOrDefault(x => x.Id == videogameId);

            var hours = videogame?.Hours;
            var status = videogame?.Status;
            var nowPlaying = videogame?.NowPlaying;
            var starred = videogame?.Starred;
            var starRating = videogame?.StarRating;
            var gameRating = videogame?.GameRating;

            VideoGameDetails videoGameDetails = new()
            {
                Hours = hours ?? 0,
                Status = status,
                NowPlaying = nowPlaying ?? false,
                Starred = starred ?? false,
                StarRating = starRating,
                GameRating = gameRating,
                AverageStarRating = averageRating?.StarRating,
                AverageGameRating = averageRating?.GameRating,
            };
            return videoGameDetails;
        }

        public async Task<bool> InsertUpdateVideoGame(BacklogEntry backlogEntry)
        {
            VideoGame videoGame = new()
            {
                Id = backlogEntry.VideoGameId,
                Name = backlogEntry.Name,
                Cover = backlogEntry.Cover,
                Genres = backlogEntry.Genres,
                Hours = backlogEntry.Hours,
                Starred = backlogEntry.Starred,
                Status = backlogEntry.Status,
                NowPlaying = backlogEntry.NowPlaying
            };

            return await _userRepository.InsertUpdateVideoGame(backlogEntry.User, videoGame);
        }

        public async Task<bool> DeleteVideoGame(int id, string userId)
        {
            return await _userRepository.DeleteVideoGame(id, userId);
        }

        public async Task<bool> RateByStars(StarRating rating)
        {
            return await _userRepository.UpdateStarRating(rating.VideoGameId, rating.User, rating.Stars);
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

            return await _userRepository.UpdateGameRating(rating.VideoGameId, rating.User, gameRating);
        }

        public async Task<IEnumerable<TrendingVideoGame>> RetrieveTrendingVideoGames()
        {
            TrendingVideoGame tvg = new();

            return new List<TrendingVideoGame> { tvg };
        }

        private VideoGameCard MapFrom(VideoGame videogame)
        {
            return new VideoGameCard()
            {
                Id = videogame.Id,
                Name = videogame.Name,
                Cover = videogame.Cover
            };
        }

        
    }
}