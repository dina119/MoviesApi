using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IMoviesService _MoviesService;
        private readonly IRateServices _RateService;
        private readonly IMapper _mapper;



        public RateController(ApplicationDbContext context, IMoviesService moviesService, IRateServices rateService, IMapper mapper)
        {
            _MoviesService = moviesService;
            _RateService = rateService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRate([FromBody] RateDto dto)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var movie = await _MoviesService.GetById(dto.MovieId);
            if (movie != null)
            {
                var rateExisist = await _RateService.CheckRateExist(dto);
                if (rateExisist == null)
                {
                    var UserRate = _mapper.Map<Rate>(dto);
                    UserRate.UserId = UserId;
                    await _RateService.AddRate(UserRate);
                    return Ok();
                }
                return BadRequest("Doublicate Rate");
            }
            else
            {
                return BadRequest("movie not exsist");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> editRate([FromBody] RateEditDto dto, int RateId)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserRate = _RateService.FindRate(RateId);
            if (UserRate.Result.UserId != UserId)
            {
                return Forbid();
            }
            UserRate.Result.RateNum = dto.RateNum;
            _RateService.UpdateRate(UserRate.Result);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieRate(int MovieID)
        {
            var MovieRates = await _RateService.GetRateByMovieId(MovieID);

            return Ok(MovieRates);

        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteRate(int RateId)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserRate = _RateService.FindRate(RateId);
            if (UserRate.Result.UserId != UserId)
            {
                return Forbid();
            }
             _RateService.RemoveRate(UserRate.Result);
            return Ok();

        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> MovieAverageRating(int movieId)
        {
            var averageRating =await _RateService.AverageRate(movieId);

            return Ok(new { AverageRating = averageRating});
        }

    }
}
