using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class MoviesService : IMoviesService
    {
         private readonly ApplicationDbContext _context;

        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Add(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
            return movie ;
        }

        public async Task<Movie> GetById(int id)
        {
            return await _context.Movies.Include(m=>m.Genre).SingleOrDefaultAsync(m=>m.ID==id);
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId=0)
        {
            return await _context.Movies.Where(m=>m.GenreId==genreId||genreId==0).OrderByDescending(m=>m.Rate).Include(m=>m.Genre).ToListAsync();
        }

        public  Movie Update(Movie movie)
        {
           _context.Update(movie);
            _context.SaveChanges();
            return movie ;
        }

        public Task<List<Movie>> Search(string? Title, int? Year, string?  genreName)
        {
        var movie = _context.Movies.AsQueryable();

    if (Title != null)
        movie = movie.Where(m => m.Title == Title);
    if (Year != null)
        movie = movie.Where(m => m.Year == Year);
    if ( genreName != null)
        movie = movie.Where(m => m.Genre.Name ==  genreName);

   return movie.Include(m=>m.Genre).ToListAsync(); // Execute query and return data.

        }

        public   Task<List<Movie>> Filter(double? rate, int? year)
        {
            var moviee=_context.Movies.AsQueryable();
            if (rate !=null) 
             moviee= moviee.Where(m=>m.Rate==rate);
            
            if (year != null)  
             moviee=  moviee.Where(m=>m.Year==year);
            
            return moviee.Include(m=>m.Genre).ToListAsync();


        }
    }
}
