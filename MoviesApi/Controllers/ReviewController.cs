using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReviewController(ApplicationDbContext context)
        {
            _context=context;
        }

        [HttpGet("{MovieId}")]
        public async Task<IActionResult> GetAllReviews(int MovieId)
        {
            var movie=_context.Movies.Find(MovieId);
            if (movie == null)
            {
                return BadRequest("Movie not found");
            }
            var reviews = await _context.Reviews
        .Where(r => r.MovieId == MovieId)
        .Include(r => r.User) // لجلب بيانات المستخدم
        .Select(r => new GettReviewsDto
        {
           Username=r.User.UserName,
           rate=r.rate,
           title=r.title,
           desription=r.desription,
           CreatedAt=r.CreatedAt
           
        })
        .ToListAsync();
         return Ok(reviews);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDto dto)
        {
            var UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value;
             if (UserId==null)
    {
        return Unauthorized("User is not logged in.");
    }
             var review=new Review
             {
              UserId=UserId,
            CreatedAt=DateTime.Now,
              rate=dto.rate,
              MovieId=dto.MovieId,
              title=dto.title,
              desription=dto.desription

             };

             
           await _context.Reviews.AddAsync(review);
            _context.SaveChanges();

            return Ok();

        }

        [Authorize]
        [HttpPut("{ReviewId}")]
        public async Task<IActionResult> EditReview(int ReviewId,[FromBody] CreateReviewDto dto)
        {
              var UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value;
             
    {
        return Unauthorized("User is not logged in.");
    }
            var review=  _context.Reviews.Find(ReviewId);
            if (review.UserId != UserId)
            {
                return Forbid();
            }
            if (review == null)
            {
                return BadRequest("Movie review not found");
            }
             
             review.UserId=UserId;
              review.CreatedAt=DateTime.Now;
              review.rate=dto.rate;
              review.MovieId=dto.MovieId;
              review.title=dto.title;
              review.desription=dto.desription

             ;
             _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpDelete("{ReviewId}")]
        public async Task<IActionResult> RemoveReview(int ReviewId)
        {
             var UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value;
   
            var review=_context.Reviews.Find(ReviewId);
             if (review.UserId != UserId)
            {
                return Forbid();
            }
            if (review == null)
            {
                return BadRequest("movie review not found");
            }
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return Ok();

        }
        

        }

    
}
