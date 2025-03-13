using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public WatchListController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMovieTOList([FromBody] AddToWatchListDto dto)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                return Unauthorized("User is not logged in.");
            }
            var IsMovieExsist = _context.watchLists.FirstOrDefault(m => m.MovieId == dto.MovieId && m.UserId == UserId);
            if (IsMovieExsist != null)
            {
                return BadRequest("Movie is already in your watch list.");
            }

            var WatchList = new WatchList
            {
                UserId = UserId,
                CreatedDate = DateTime.Now,
                MovieId = dto.MovieId,

            };
            await _context.watchLists.AddAsync(WatchList);
            _context.SaveChanges();
            return Ok(WatchList);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getWatchList()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                return Unauthorized("User is not logged in.");
            }
            var WatchList =await _context.watchLists.Where(w => w.UserId == UserId).Include(w => w.Movie)
                .Select(w => new
                {
                    w.Movie.Title,
                    w.Movie.Genre,
                    w.Movie.Year
                }


                ).ToListAsync();
            return Ok(WatchList);
            // by user id
            //return Movie Name,date of add to list
        }

        [Authorize]
        [HttpDelete]
         public async Task<IActionResult> removeMovie(int MovieId)
        {
             var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                return Unauthorized("User is not logged in.");
            }
            var WathList=await _context.watchLists.FirstOrDefaultAsync(m=>m.MovieId==MovieId&&m.UserId==UserId);
            if (WathList == null)
            {
              return BadRequest("Movie not exsit in watch list");
            }
            
                _context.watchLists.Remove(WathList);
                _context.SaveChanges();
            
            return Ok();
        }
        
        
    }
}
