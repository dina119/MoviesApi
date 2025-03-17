using MoviesApi.Dto;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IRateServices
    {
        Task<List<GetRatesDto>> GetRateByMovieId(int MovieId);
        Task<Rate>AddRate(Rate rate);
        Task<Rate>FindRate(int id);
        Task<Rate> CheckRateExist(RateDto dto);
        Task<double>AverageRate(int Movieid);
        Rate  UpdateRate(Rate rate);
        Rate  RemoveRate(Rate rate);

    }
}
