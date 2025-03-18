namespace MoviesApi.Dto
{
    public class MovieDetailsDto
    {
        public int ID { get;set;}
        public string Title { get;set;}
        public int Year { get;set;}
       // public double Rate { get;set;}
        public string StoreLine { get;set;}
        public string PosterUrl { get;set;}
        public byte GenreId { get;set;}
        public string GenreName { get;set;} // تأكدي من عدم حفظها في قاعدة البيانات
       public double AverageRate { get; set; }

    }
}
