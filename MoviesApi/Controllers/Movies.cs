using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Dynamic;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     [Authorize]
    public class Movies : ControllerBase
    {
        private readonly IMoviesService _MoviesService;
        private readonly IGenresService _GenresService;
        private readonly IMapper _mapper;
        private new List<string> _allowedExtention=new List<string> {".jpg",".png" };
        private long _MaxAllowedSize=1048576; //1M 1024*1024

        public Movies(IMoviesService MoviesService, IGenresService GenresService, IMapper mapper)
        {
            _MoviesService = MoviesService;
            _GenresService = GenresService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(){

            var movie=await _MoviesService.GetAll();
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movie);
            return Ok(data);
        }

         [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){

            var movie=await _MoviesService.GetById(id);
            if(movie==null)
                return NotFound($"item not found with id :{id}");
              var  dto=_mapper.Map<MovieDetailsDto>(movie);
           
            return Ok(dto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreId(byte genreId)
        {
            var movie = await _MoviesService.GetAll(genreId);
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movie);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateAsync([FromForm]CreateMoviesDto dto){

            //Allow specific file extention to upload
            if(!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            return BadRequest($"only jpg,png allowed !");

            //Allow limited size
            if(_MaxAllowedSize>1048576)
                return BadRequest($"the Max file size allowed is 1M !");

            // To valied GenreID
            var IsValidGenre = await _GenresService.IsValidGenre(dto.GenreId);
            if(!IsValidGenre)
                return BadRequest($"Invaild genre id !");

             using var dataStream=new MemoryStream();
             await dto.Poster.CopyToAsync(dataStream);
           var movie=_mapper.Map<Movie>(dto);
            movie.Poster=dataStream.ToArray();
           _MoviesService.Add(movie);
            return Ok (movie);
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm]CreateMoviesDto dto)
        {
             // To valied GenreID
             var IsValidGenre = await _GenresService.IsValidGenre(dto.GenreId);
            if(!IsValidGenre)
                return BadRequest($"Invaild genre id !");

        var movie=await _MoviesService.GetById(id);
            if (dto.Poster != null) { 
            //Allow specific file extention to upload
            if(!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            return BadRequest($"only jpg,png allowed !");

            //Allow limited size
            if(_MaxAllowedSize>1048576)
                return BadRequest($"the Max file size allowed is 1M !");

             using var dataStream=new MemoryStream();
             await dto.Poster.CopyToAsync(dataStream);
            }

            movie.Title=dto.Title;
            movie.Year=dto.Year;
            movie.Rate=dto.Rate;
            movie.GenreId=dto.GenreId;

            _MoviesService.Update(movie);
         return Ok(movie);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> deleteAsync(int id)
        {
         var movie = await _MoviesService.GetById(id);
            if(movie==null)
                return NotFound($"item not found with id :{id}");
            _MoviesService.Delete(movie);
            return Ok();
        }
    }
}
