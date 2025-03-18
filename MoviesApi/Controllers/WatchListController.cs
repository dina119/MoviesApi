using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly IWatchListServices _IWatchListServices;
        private readonly IMoviesService _IMoviesService;
         private readonly IMapper _mapper;
        public WatchListController(ApplicationDbContext context, IWatchListServices iWatchListServices, IMapper mapper, IMoviesService iMoviesService)
        {
            _IWatchListServices = iWatchListServices;
            _mapper = mapper;
            _IMoviesService = iMoviesService;
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
            var x=await _IMoviesService.GetById(dto.MovieId);
            if (x == null)
            {
                return BadRequest("this Movie not exist in database.");
            }
            var IsMovieExsist =await _IWatchListServices.CheckMovieExist(dto);
            
            if (IsMovieExsist != null)
            {
                return BadRequest("Movie is already in your watch list.");
            }

            var WatchList =_mapper.Map<WatchList>(dto);
            WatchList.CreatedDate=DateTime.Now;
            WatchList.UserId=UserId;
            _IWatchListServices.AddToWatchList(WatchList);
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
            var WatchList = await _IWatchListServices.GetWatchList();

            return Ok(WatchList);
           
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> removeMovie(AddToWatchListDto dto)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                return Unauthorized("User is not logged in.");
            }
          
            var watchList=await _IWatchListServices.CheckMovieExist(dto);
            if (watchList == null)
            {
                return BadRequest("Movie not exsit in watch list");
            }

           await  _IWatchListServices.RemoveFromList(watchList);

            return Ok(watchList);
        }


    }
}
