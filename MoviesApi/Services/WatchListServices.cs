using MoviesApi.Dto;
using MoviesApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services
{
    public class WatchListServices :IWatchListServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _Access;

        public WatchListServices(ApplicationDbContext context, IHttpContextAccessor access)
        {
            _context = context;
            _Access = access;
        }

        public  async Task<WatchList> AddToWatchList(WatchList watchList)
        {
            _context.watchLists.Add(watchList);
            _context.SaveChanges();
            return watchList;
        }

        public async Task<WatchList> CheckMovieExist(AddToWatchListDto dto)
        {
            var UserId = _Access.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
         var IsMovieEsist= await  _context.watchLists.FirstOrDefaultAsync(m => m.MovieId == dto.MovieId && m.UserId == UserId);
            return IsMovieEsist;
        }

        public async Task<List<WatchListDto>> GetWatchList()
        {
            var UserId = _Access.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var WatchList = 
                await _context.watchLists.Where(w => w.UserId == UserId).Include(w => w.Movie).Select(w=>
                    new WatchListDto
                    {
                        Genre=w.Movie.Genre.Name,
                        Title=w.Movie.Title,
                        Year=w.Movie.Year
                    }).ToListAsync();
            return WatchList;
        }

        public async Task<WatchList> RemoveFromList(WatchList watchList)
        {
            _context.watchLists.Remove(watchList);
           await _context.SaveChangesAsync();
            return watchList;
        }
    }
}
