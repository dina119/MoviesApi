using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        
        private readonly IReviewService _reviewService;
        public ReviewController(ApplicationDbContext context, IReviewService reviewService)
        {
           
            _reviewService = reviewService;
        }

        [HttpGet("{MovieId}")]
        public async Task<IActionResult> GetAllReviews(int MovieId)
        {
            
            var reviews = await _reviewService.GetReviewsByMovieId(MovieId);
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

             
           await _reviewService.AddReview(review);

            return Ok();

        }

        [Authorize]
        [HttpPut("{ReviewId}")]
        public async Task<IActionResult> EditReview(int ReviewId,[FromBody] CreateReviewDto dto)
        {
              var UserId=User.FindFirst(ClaimTypes.NameIdentifier).Value;
         if (UserId==null)     
    {
        return Unauthorized("User is not logged in.");
    }
            var review=await  _reviewService.FindReview(ReviewId);
            if (review.UserId!= UserId)
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
             _reviewService.UpdateReview(review);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{ReviewId}")]
        public async Task<IActionResult> RemoveReview(int ReviewId)
        {
             var UserId= User.FindFirst(ClaimTypes.NameIdentifier).Value;
   
            var review=await _reviewService.FindReview(ReviewId);
             if (review.UserId != UserId)
            {
                return Forbid();
            }
            if (review == null)
            {
                return BadRequest("movie review not found");
            }
            _reviewService.RemoveReview(review);
            return Ok();

        }
        

        }

    
}
