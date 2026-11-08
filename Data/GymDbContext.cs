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
        // Indicamos las clases de modelo para mapearlas a tablas de SQL Server
        public DbSet<Activity> Activities { get; set; }
        // Añade el resto (piensa cuales debes poner para que se conviertan en tablas)
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Instructor> Intructors { get; set; }
    }

}
