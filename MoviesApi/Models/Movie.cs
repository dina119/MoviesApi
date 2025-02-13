using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class Movie
    {
        public int ID { get;set;}
        [MaxLength(250)]
        public string Title { get;set;}
        public int Year { get;set;}
        public byte Rate { get;set;}
        [MaxLength(250)]
        public string StoreLine { get;set;}
        public string PosterUrl { get;set;}
        public byte GenreId { get;set;}
        public Genre Genre { get;set;}

    }
}
