using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResWeb.Models;

namespace ResWeb.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleDateTime> ScheduleDateTimes { get; set; }
    }

}
