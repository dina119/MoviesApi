namespace MoviesApi.Models
{
    public class WatchList
    {
        public int id { get;set;}
        public int MovieId { get;set;}
        public string UserId { get;set;}
        public DateTime CreatedDate { get;set;}=DateTime.UtcNow;
        public virtual Movie Movie { get; set; }
         public virtual ApplicationUser User { get;set;}
    }
}
