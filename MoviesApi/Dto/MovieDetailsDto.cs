namespace MoviesApi.Dto
{
    public class MovieDetailsDto
    {
        public int ID { get;set;}
        public string Title { get;set;}
        public int Year { get;set;}
        public byte Rate { get;set;}
        public string StoreLine { get;set;}
        public Byte[] Poster { get;set;}
        public byte GenreId { get;set;}
        public string GenreName { get;set;}

    }
}
