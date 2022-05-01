using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Application.Models
{
    public class BacklogEntry
    {
        public string User { get; set; } = string.Empty;
        public int VideoGameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public List<string>? Genres { get; set; }
        public int Hours { get; set; }
        public bool Starred { get; set; }
        public bool NowPlaying { get; set; }
        public Status Status { get; set; }
    }
}
