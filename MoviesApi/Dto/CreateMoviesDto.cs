using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dto
{
    public class CreateMoviesDto
    {
       

        [MaxLength(250)]
        public string Title { get;set;}
        public int Year { get;set;}
        [MaxLength(250)]
        public string StoreLine { get;set;}
        public IFormFile? Poster { get;set;}
        public byte GenreId { get;set;}
    }
}
