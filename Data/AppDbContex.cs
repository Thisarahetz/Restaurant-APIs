using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using restaurant_app_API.Entity;

namespace postgreanddotnet.Data
{
    public class AppDbContex : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContex(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> Users { get; set; }
        // public DbSet<UserId> UserIds { get; set; }


    }
}
