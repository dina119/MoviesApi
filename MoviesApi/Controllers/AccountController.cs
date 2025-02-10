using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Dto;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager=userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto UserDto)
        {
            ApplicationUser user=new ApplicationUser();
            user.UserName=UserDto.UserName;
            user.Email=UserDto.Email;
            if (ModelState.IsValid) { 
            IdentityResult result= await _userManager.CreateAsync(user,UserDto.Password);
                if (result.Succeeded) { 
                    return Ok("Account Sign up succes");
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }

            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
