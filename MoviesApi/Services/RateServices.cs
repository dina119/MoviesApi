using MoviesApi.Dto;
using MoviesApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services
{
    public class RateServices : IRateServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _Access;
         private readonly IServiceScopeFactory _serviceScopeFactory;
        public RateServices(ApplicationDbContext context, IHttpContextAccessor access, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _Access = access;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Rate> AddRate(Rate rate)
        {
             _context.Rates.Add(rate);
            _context.SaveChanges();
            return rate;

        }

        public async Task<double> AverageRate(int Movieid)
        {
          using (var scope = _serviceScopeFactory.CreateScope()) 
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var result = await context.Rates
                .Where(r => r.MovieId == Movieid)
                .AverageAsync(r => (double?)r.RateNum) ?? 0;

            return result;
        }
        }

        public async Task<Rate> CheckRateExist(RateDto dto)
        {
           
            var UserId=  _Access.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
             var rateExisist=await _context.Rates.FirstOrDefaultAsync(r=>r.UserId==UserId&&r.MovieId==dto.MovieId);
            return rateExisist;
        }

        public async Task<Rate> FindRate(int id)
        {
           return await _context.Rates.FindAsync(id);
        }

        public async Task<List<GetRatesDto>> GetRateByMovieId(int MovieId)
        {
             var MovieRates = _context.Rates.Where(r => r.MovieId == MovieId).Include(r => r.User).Select(r =>
            new GetRatesDto
            {
                RateNum = r.RateNum,
                UserId = r.UserId,
                Username = r.User.UserName
            }
            ).ToList();
            return MovieRates;
        }

        public Rate RemoveRate(Rate rate)
        {
            _context.Rates.Remove(rate);
            _context.SaveChanges();
            return rate;
        }

        public Rate UpdateRate(Rate rate)
        {
            _context.Update(rate);
            _context.SaveChanges();
            return rate;
        }
    }
}
