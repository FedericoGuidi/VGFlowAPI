using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Application.Models
{
    public class TrendingVideoGame
    {
        public int Id { get; set; }
        public string Cover { get; set; } = string.Empty;
        public double AverageStarRating { get; set; }
        public int TotalNowPlaying { get; set; }
        public double BestGameRating { get; set; }
    }
}
