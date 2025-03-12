namespace MoviesApi.Models
{
    public class Review
    {
     public int id { set;get;}
     public string UserId { get; set; }
     public string title { set;get;}
     public string desription { set;get;}
     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     public double rate { set;get;}
     public int MovieId { get; set; }
     public virtual Movie Movie { get; set; }
     public virtual ApplicationUser User { get; set; }

    }
}
