using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> AddReview(Review review)
        {
           await _context.Reviews.AddAsync(review);
            _context.SaveChanges();
            return review;
        }

        public async Task<Review> FindReview(int id)
        {
          return await _context.Reviews.FindAsync(id);
        }

        public async  Task<List<GettReviewsDto>> GetReviewsByMovieId(int MovieId)
        {
          var review=await  _context.Reviews
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
            return review;
        }

        public Review UpdateReview(Review review)
        {
         _context.Update(review);
          _context.SaveChanges();
            return review;
        }

        public Review RemoveReview(Review review)
        {
         _context.Remove(review);
          _context.SaveChanges();
            return review;
        }
    }
}
