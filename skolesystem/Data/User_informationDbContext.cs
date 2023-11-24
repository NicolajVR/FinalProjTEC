using Microsoft.EntityFrameworkCore;
using skolesystem.Models;

namespace skolesystem.Data
{
    /*
    User_informationDbContext er en klasse, der repræsenterer databasenkonteksten for user_information entiteten.

    public User_informationDbContext(DbContextOptions<User_informationDbContext> options) : base(options)
    - Konstruktør, der initialiserer User_informationDbContext med DbContextOptions og kalder den overordnede klasse (base class).

    public DbSet<User_information> User_information { get; set; }
    - DbSet, der repræsenterer tabellen for user_information i databasen.
*/
    public class User_informationDbContext : DbContext
    {
        public User_informationDbContext(DbContextOptions<User_informationDbContext> options) : base(options)
        {

        }

        public DbSet<User_information> User_information { get; set; }
    }
}