using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dto
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get;set;}
         [Required]
         public string Password{ get;set;}
    }
}
