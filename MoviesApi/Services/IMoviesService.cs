using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IMoviesService
    {
     public Task<IEnumerable<Movie>> GetAll(byte genreId=0);
     public Task<Movie> GetById(int id);
     public Task<Movie> Add(Movie movie);
     public Movie Update(Movie movie);
     public Movie Delete(Movie movie);
    }
}
