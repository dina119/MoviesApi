using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dto
{
    public class UserRoleDto
    {
        [Required]
        public string UserId { get;set;}

        [Required]
        public string Role { get;set;}
    }
}
