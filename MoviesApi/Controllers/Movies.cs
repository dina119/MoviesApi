using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Dto;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class Movies : ControllerBase
    {
        private readonly IMoviesService _MoviesService;
        private readonly IGenresService _GenresService;
        private readonly IMapper _mapper;
        // private readonly ApplicationDbContext _context;
        private new List<string> _allowedExtention = new List<string> { ".jpg", ".png" };
        private long _MaxAllowedSize = 1048576; //1M 1024*1024

        public Movies(IMoviesService MoviesService, IGenresService GenresService, IMapper mapper, ApplicationDbContext context)
        {
            _MoviesService = MoviesService;
            _GenresService = GenresService;
            _mapper = mapper;
           // _context = context;
        }

        [HttpGet]
       
        public async Task<IActionResult> GetAllAsync()
        {

            var movie = await _MoviesService.GetAll();
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movie);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var movie = await _MoviesService.GetById(id);
            if (movie == null)
                return NotFound($"item not found with id :{id}");
            var dto = _mapper.Map<MovieDetailsDto>(movie);

            return Ok(dto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreId(byte genreId)
        {
            var movie = await _MoviesService.GetAll(genreId);
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movie);
            return Ok(data);
        }

        [HttpGet("Search")]
         public async Task<IActionResult> SearchMovies(string? Title, int? Year,string? genreName)
        {
         var movie = await _MoviesService.Search(Title,Year, genreName);
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movie);
            return Ok(data);
        }

        [HttpGet("Filter")]
         public async Task<IActionResult> FilterMovies(double? rate,int? year)
        {

            var movie = await _MoviesService.Filter(rate,year);
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movie);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateMoviesDto dto)
        {

            //Allow specific file extention to upload
            if (!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest($"only jpg,png allowed !");

            //Allow limited size
            if (_MaxAllowedSize > 1048576)
                return BadRequest($"the Max file size allowed is 1M !");
            string imagePath = null;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Poster.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Poster.CopyToAsync(stream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            imagePath = $"{baseUrl}/uploads/{uniqueFileName}";

            // To valied GenreID
            var IsValidGenre = await _GenresService.IsValidGenre(dto.GenreId);
            if (!IsValidGenre)
                return BadRequest($"Invaild genre id !");
            var movie = _mapper.Map<Movie>(dto);
            movie.PosterUrl = imagePath;
            _MoviesService.Add(movie);
            return Ok(movie);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] CreateMoviesDto dto)
        {
            // To valied GenreID
            var IsValidGenre = await _GenresService.IsValidGenre(dto.GenreId);
            if (!IsValidGenre)
                return BadRequest($"Invaild genre id !");

            var movie = await _MoviesService.GetById(id);
            if (dto.Poster != null)
            {
                //Allow specific file extention to upload
                if (!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest($"only jpg,png allowed !");

                //Allow limited size
                if (_MaxAllowedSize > 1048576)
                    return BadRequest($"the Max file size allowed is 1M !");


            }
            string imagePath = null;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Poster.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Poster.CopyToAsync(stream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            imagePath = $"{baseUrl}/uploads/{uniqueFileName}";

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.GenreId = dto.GenreId;
            movie.PosterUrl = imagePath;
            _MoviesService.Update(movie);
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> deleteAsync(int id)
        {
            var movie = await _MoviesService.GetById(id);
            if (movie == null)
                return NotFound($"item not found with id :{id}");
            _MoviesService.Delete(movie);
            return Ok();
        }
    }
}
