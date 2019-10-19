using Microsoft.EntityFrameworkCore;
 
namespace BrightIdea.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Post> Posts {get;set;}
        public DbSet<Like> Likes {get;set;}
    }
}