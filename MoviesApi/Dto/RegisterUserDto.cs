using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dto
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get;set;}
        public string Email { get;set;}
         [Required]
        public string Password { get;set;}
         [Required]
        [Compare("Password")]
        public string ConfirmPassword{ get;set;}
    }
}
