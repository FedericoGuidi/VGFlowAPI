namespace UsersAPI.Application.Models
{
    public class StarRating
    {
        public string User { get; set; } = string.Empty;
        public int VideoGameId { get; set; }
        public double Stars { get; set; }
    }
}
