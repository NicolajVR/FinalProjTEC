using Microsoft.EntityFrameworkCore;
using skolesystem.Models;

namespace skolesystem.Data
{
    public class UsersDbContext : DbContext
    {
        /*
     UsersDbContext er en klasse, der repræsenterer databasenkonteksten for Users entiteten.

     public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
     - Konstruktør, der initialiserer UsersDbContext med DbContextOptions og kalder den overordnede klasse (base class).

     public DbSet<Users> Users { get; set; }
     - DbSet, der repræsenterer tabellen for Users i databasen.
 */
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
    }
}