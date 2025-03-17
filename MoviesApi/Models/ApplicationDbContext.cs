using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace MoviesApi.Models
    
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
            
        }
        public DbSet<Genre> Genres { get;set;}
        public DbSet<Movie> Movies { get;set;}
        public DbSet<Review> Reviews { get;set;}
        public DbSet<WatchList> watchLists { get;set;}
        public DbSet<Rate> Rates { get;set;}

    }
}
