namespace MoviesApi.Dto
{
    public class CreateReviewDto
    {

     public string title { set;get;}
     public string desription { set;get;}
     public double rate { set;get;}
     public int MovieId { get; set; }
     
    }
}
