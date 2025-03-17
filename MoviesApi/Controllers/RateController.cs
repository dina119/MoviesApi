using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        

        public RateController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRate([FromBody]RateDto dto)
        {
             var UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var movie=_context.Movies.Find(dto.MovieId);
           // var rate=_context.Rates.Where(r=>r.MovieId==dto.MovieId&&r.UserId==UserId).ToList();
            if (movie != null) { 
                var rateExisist=_context.Rates.FirstOrDefault(r=>r.UserId==UserId&&r.MovieId==dto.MovieId);
                if (rateExisist == null) { 
             var UserRate=new Rate
             {
                 MovieId=dto.MovieId,
                 RateNum=dto.RateNum,
                 UserId=UserId,
                 
             };
            _context.Rates.Add(UserRate);
            _context.SaveChanges();
            return Ok();
                }
                return BadRequest();
            }
            else
            {
                return BadRequest("movie not exsist");
            }
        }

        [Authorize]
        [HttpPut]
        public  async Task<IActionResult> editRate([FromBody]RateEditDto dto,int RateId)
        {
             var UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserRate=_context.Rates.Find(RateId);
            
            if (UserRate.UserId!=UserId)
            {
                return Forbid();

            }
            UserRate.RateNum=dto.RateNum;
            
            _context.Update(UserRate);
            _context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetMovieRate(int MovieID)
        {
            var MovieRates = _context.Rates.Where(r => r.MovieId == MovieID).Include(r=>r.User).Select(r=>
            new GetRatesDto
            {
                RateNum=r.RateNum,
                UserId=r.UserId,
                Username=r.User.UserName
            }
            ).ToList();
            return Ok(MovieRates);
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteRate(int RateId)
        {
             var UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value;
           // var UserRate=_context.Rates.Where(r=>r.id==RateId&&r.UserId==UserId);
            var UserRate=_context.Rates.Find(RateId);
            if (UserRate.UserId != UserId)
            {
                return Forbid();
            }
           _context.Rates.Remove(UserRate);
            _context.SaveChanges();
            return Ok();

        }

        [HttpGet("{movieId}")]
public async Task<IActionResult> GetMovieRating(int movieId)
{
    var averageRating = await _context.Rates
        .Where(r => r.MovieId == movieId)
        .AverageAsync(r => (double?)r.RateNum) ?? 0;

    return Ok(new { AverageRating = averageRating });
}

    }
}
