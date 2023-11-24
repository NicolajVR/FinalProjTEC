using Microsoft.EntityFrameworkCore;
using skolesystem.Models;

namespace skolesystem.Data
{
    /*
    ScheduleDbContext er en klasse, der repræsenterer databasenkonteksten for skemaentiteten.

    public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
    - Konstruktør, der initialiserer ScheduleDbContext med DbContextOptions og kalder den overordnede klasse (base class).

    public DbSet<Schedule> Schedule { get; set; }
    - DbSet, der repræsenterer tabellen for skema i databasen.
*/
    public class ScheduleDbContext : DbContext
    {
        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
        {

        }

        public DbSet<Schedule> Schedule { get; set; }
    }
}