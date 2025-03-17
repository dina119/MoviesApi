namespace MoviesApi.Dto
{
    public class GetRatesDto
    {
        
        public string Username { get; set; }  
         public string UserId { get; set; }
         public int  RateNum { get;set;}//From 1 to 5
    }
}
