using MoviesApi.Dto;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IReviewService
    {
        Task<List<GettReviewsDto>> GetReviewsByMovieId(int MovieId);
        Task<Review>AddReview(Review review);
        Task<Review>FindReview(int id);
        Review  UpdateReview(Review review);
        Review  RemoveReview(Review review);
    }
}
