using _4VGymAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace _4VGymAPI.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
    }

}
