namespace MoviesApi.Models
{
    public class Rate
    {
     public int id { set;get;}
     public string UserId { get; set; }
    public int  RateNum { get;set;}//From 1 to 5
     public int MovieId { get; set; }
     public virtual Movie Movie { get; set; }
     public virtual ApplicationUser User { get; set; }
    }
}
