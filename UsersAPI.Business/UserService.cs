using UsersAPI.Application.Business;
using UsersAPI.Application.Models;
using UsersAPI.Application.Providers;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRatingRepository _ratingRepository;
        public UserService(IUserRepository userRepository, IRatingRepository ratingRepository)
        {
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
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

            profile.NowPlaying = user.VideoGames?.Where(x => x.NowPlaying).Select(x => MapFrom(x)).ToList();
            profile.Favorites = user.VideoGames?.Where(x => x.Starred).Select(x => MapFrom(x)).ToList();

            return profile;
        }

        public async Task<VideoGameDetails> RetrieveVideoGameDetailsAsync(int videogameId, string userId)
        {
            // Dettagli relativi all'utente
            Rating userRating = userRating = await _ratingRepository.RetrieveAsync(videogameId, userId);
            User user = await _userRepository.RetrieveAsync(userId);
            
            // Dettagli relativi ai rating di tutti gli utenti
            List<Rating> ratings = await _ratingRepository.RetrieveManyAsync(videogameId);

            var videogame = user.VideoGames?.FirstOrDefault(x => x.Id == videogameId);

            var hours = videogame?.Hours;
            var status = videogame?.Status;
            var nowPlaying = videogame?.NowPlaying;
            var starred = videogame?.Starred;
            var ratingsCount = ratings.Count;
            var gameRatings = ratings.Select(x => x.GameRating);

            GameRating averageGameRating = gameRatings.Any() ? gameRatings.GroupBy(gameRatings => 1)
                .Select(r => new GameRating()
                {
                    Gameplay = r.Sum(x => x!.Gameplay) / ratingsCount,
                    Plot = r.Sum(x => x!.Plot) / ratingsCount,
                    Music = r.Sum(x => x!.Music) / ratingsCount,
                    Graphics = r.Sum(x => x!.Graphics) / ratingsCount,
                    LevelDesign = r.Sum(x => x!.LevelDesign) / ratingsCount,
                    Longevity = r.Sum(x => x!.Longevity) / ratingsCount,
                    IA = r.Sum(x => x!.IA) / ratingsCount,
                    Physics = r.Sum(x => x!.Physics) / ratingsCount
                })
                .Single() : new GameRating();

            double? averageStarRating = ratings.Any() ? ratings.Sum(x => x.StarRating ?? 0) / ratingsCount : null;

            VideoGameDetails videoGameDetails = new()
            {
                Hours = hours ?? 0,
                Status = status,
                NowPlaying = nowPlaying ?? false,
                Starred = starred ?? false,
                StarRating = userRating.StarRating,
                GameRating = userRating.GameRating,
                AverageStarRating = averageStarRating,
                AverageGameRating = averageGameRating
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