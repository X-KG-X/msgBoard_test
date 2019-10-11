using Microsoft.EntityFrameworkCore;
 
namespace msgBoard.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Message> Messages {get;set;}
        public DbSet<Comment> Comments {get;set;}
    }
}