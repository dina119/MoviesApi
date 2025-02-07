using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre>Add(Genre Genre);
        Task<Genre>GetById(byte id);
        Genre Update(Genre Genre);
        Genre Delete(Genre Genre);
         Task<bool>IsValidGenre(byte id);
    }
}
