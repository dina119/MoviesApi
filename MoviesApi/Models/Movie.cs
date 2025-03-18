using MoviesApi.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApi.Models
{
    public class Movie
    {
       
        public int ID { get;set;}
        [MaxLength(250)]
        public string Title { get;set;}
        public int Year { get;set;}
        [MaxLength(250)]
        public string StoreLine { get;set;}
        public string PosterUrl { get;set;}
        public byte GenreId { get;set;}
        public Genre Genre { get;set;}
         public virtual List<Review> Reviews { get; set; }
        [NotMapped]
    public double AverageRate { get; set; }
        
    }
}
