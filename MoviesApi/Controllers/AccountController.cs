using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Dto;
using MoviesApi.Models;
using NuGet.Common;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            this.config = config;
            _RoleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto UserDto)
        {
           if(await _userManager.FindByEmailAsync(UserDto.Email) != null)
            {
                return  BadRequest(new {Message="Email is alerdy in use"});
            }
            else { 
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

         [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto UserDto)
        {
            if (ModelState.IsValid)
            {
           ApplicationUser user= await _userManager.FindByNameAsync(UserDto.UserName);
               if (user != null)
                {
                    bool found =await _userManager.CheckPasswordAsync(user,UserDto.Password);
                    if (found)
                    {
                        //create JWT

                        // claims Token
                        var claims=new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name,user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
                        //get role 
                        var roles= await _userManager.GetRolesAsync(user);
                        foreach (var item in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role,item));
                        }
                        //signingCredentials
                        SecurityKey key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"])) ;
                        SigningCredentials signingCred =new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                        
                        //create Token
                        JwtSecurityToken mytoken =new JwtSecurityToken(
                            issuer: config["JWT:Issuer"], //URL web api
                            audience: config["JWT:Audience"], //URL for consumer
                            claims:claims,
                            expires:DateTime.Now.AddHours(1),
                            signingCredentials:signingCred
                            );
                        return Ok(new
                        {
                            Token =new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration=mytoken.ValidTo
                        }

                            );
                    }
                    
                }
               return Unauthorized();
            }
            return Unauthorized();
        }

        [HttpPost("AssignRole")]
        [Authorize(Roles ="Admin")]
         public async Task<IActionResult> AssignRoles(UserRoleDto Userdto)
        {
          var user=  await _userManager.FindByIdAsync(Userdto.UserId);
            if(user==null||!await _RoleManager.RoleExistsAsync(Userdto.Role))
                return BadRequest("In valid user id or role");
            if(await _userManager.IsInRoleAsync(user,Userdto.Role))
                return BadRequest("User allready assigned to this role");
          var result= await _userManager.AddToRoleAsync(user,Userdto.Role);
            return Ok(Userdto);
        }
    }
}
