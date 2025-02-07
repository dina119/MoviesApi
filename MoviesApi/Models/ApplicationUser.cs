using Microsoft.AspNetCore.Identity;
using NuGet.LibraryModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required,MaxLength(50)]
        public string FirstName { get;set;}

        [Required,MaxLength(50)]
        public string LastName { get;set;}
        //hi malk

    }
}
