using Microsoft.EntityFrameworkCore;
using skolesystem.Models;

namespace skolesystem.Data
{
    /*
    AbsenceDbContext er en klasse, der repræsenterer databasenkonteksten for fraværsentiteten.

    public DbSet<Absence> Absence { get; set; }
    - DbSet, der repræsenterer tabellen for fravær i databasen.

    public AbsenceDbContext(DbContextOptions<AbsenceDbContext> options) : base(options)
    - Konstruktør, der initialiserer AbsenceDbContext med DbContextOptions og kalder den overordnede klasse (base class).

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    - Metode, der overskriver standardimplementeringen af OnModelCreating fra DbContext.
    - modelBuilder.Entity<Absence>().HasKey(a => a.absence_id);
      - Definerer, at fraværentiteten har en primær nøgle (PrimaryKey) på absence_id.
    
    base.OnModelCreating(modelBuilder);
    - Kalder den overordnede klasse (base class) for at sikre, at eventuelle yderligere konfigurationer fra forældreklassen bevares.

*/
    public class AbsenceDbContext : DbContext
    {
        public DbSet<Absence> Absence { get; set; }

        public AbsenceDbContext(DbContextOptions<AbsenceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Absence>().HasKey(a => a.absence_id);

            base.OnModelCreating(modelBuilder);
        }
    }
}