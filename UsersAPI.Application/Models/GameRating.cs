namespace UsersAPI.Application.Models
{
    public class UserGameRating
    {
        public string User { get; set; } = string.Empty;
        public int VideoGameId { get; set; }
        public int Gameplay { get; set; }
        public int Plot { get; set; }
        public int Music { get; set; }
        public int Graphics { get; set; }
        public int LevelDesign { get; set; }
        public int Longevity { get; set; }
        public int IA { get; set; }
        public int Physics { get; set; }
    }
}
