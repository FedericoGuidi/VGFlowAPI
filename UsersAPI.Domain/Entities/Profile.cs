using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.Domain.Entities
{
    public class Profile
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Social>? Social { get; set; }
        public Backlog Backlog { get; set; } = new Backlog();
        public List<VideoGameCard>? NowPlaying { get; set; }
        public List<VideoGameCard>? Favorites { get; set; }
    }

    public class Backlog
    {
        public int TotalGames { get; set; }
        public int TotalHours { get; set; }
        public string MostPlayedGenre { get; set; } = string.Empty;
    }
}
