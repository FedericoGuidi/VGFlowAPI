using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Domain.Entities
{
    public class VideoGameDetails
    {
        public int Hours { get; set; }
        public Status? Status { get; set; }
        public bool NowPlaying { get; set; }
        public bool Starred { get; set; }
        public double? StarRating { get; set; }
        public double? AverageStarRating { get; set; }
        public GameRating? GameRating { get; set; }
        public GameRating? AverageGameRating { get; set; }
    }
}
