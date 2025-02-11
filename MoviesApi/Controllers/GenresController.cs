using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dto;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _GenresService;
        public GenresController(IGenresService GenresService)
        {
            _GenresService=GenresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(){
            var genres=await _GenresService.GetAll();
            return Ok (genres);
        }
        
        [HttpPost]
        [Authorize]
         public async Task<IActionResult> CreteAllAsync(CreateGenersDto dto){

         var genre =new Genre
         {
             Name=dto.Name
         };
           await _GenresService.Add(genre);

            return Ok(genre);
            }

        [HttpPut("{id}")]
         public async Task<IActionResult> UpdateAsync(byte id,[FromBody]CreateGenersDto dto)
        {
            var genre=await _GenresService.GetById(id);
            if(genre==null)
                return NotFound($"ID : {id} not fount to update");
            genre.Name=dto.Name;
            _GenresService.Update(genre);
            return Ok(genre);
                
        }

        [HttpDelete("{id}")]
         public async Task<IActionResult> Delete(byte id)
        {
            var genre=await _GenresService.GetById(id);
            if(genre==null)
                return NotFound($"ID : {id} not fount to update");
            _GenresService.Delete(genre);
            return Ok();
                
        }
}
}
