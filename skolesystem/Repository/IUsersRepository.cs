using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.Models;

namespace skolesystem.Repository
{
    /*
    public interface IUsersRepository
    - Interface, der definerer metoder til at udføre operationer på Users-objekter i databasen.

    Task<Users> GetById(int id);
    - Henter et Users-objekt baseret på den angivne id.

    Task<IEnumerable<Users>> GetAll();
    - Henter alle Users-objekter fra databasen.

    Task<IEnumerable<Users>> GetDeletedUsers();
    - Henter alle slettede Users-objekter fra databasen.

    Task AddUser(Users user);
    - Tilføjer et nyt Users-objekt til databasen.

    Task UpdateUser(Users user);
    - Opdaterer et eksisterende Users-objekt i databasen.

    Task SoftDeleteUser(int id);
    - Udfører en "soft delete" af et Users-objekt i databasen.

    Task<Users> GetBySurname(string surname);
    - Henter et Users-objekt baseret på det angivne efternavn.
*/
    public interface IUsersRepository
    {
        Task<Users> GetById(int id);
        Task<IEnumerable<Users>> GetAll();
        Task<IEnumerable<Users>> GetDeletedUsers();
        Task AddUser(Users user);
        Task UpdateUser(Users user);
        Task SoftDeleteUser(int id);
        Task<Users> GetBySurname(string surname);
    }


    /*
    public class UsersRepository : IUsersRepository
    - Implementering af IUsersRepository-interfacet. Indeholder metoder til at udføre operationer på Users-objekter i databasen.

    private readonly UsersDbContext _context;
    - En privat reference til UsersDbContext, som bruges til at interagere med databasen.

    public UsersRepository(UsersDbContext context)
    - Konstruktør, der modtager en instans af UsersDbContext og tildeler den til det private _context-felt.
*/
    public class UsersRepository : IUsersRepository
    {
        private readonly UsersDbContext _context;

        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        /*
    public async Task<Users> GetBySurname(string surname)
    - Metode, der søger efter en bruger i databasen baseret på surname.

    return await _context.Users.FirstOrDefaultAsync(u => u.surname == surname);
    - Bruger Entity Framework's FirstOrDefaultAsync-metode til at finde den første bruger, hvis surname matcher det angivne surname.
    - Returnerer null, hvis der ikke findes en bruger med det angivne surname.
*/
        public async Task<Users> GetBySurname(string surname)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.surname == surname);
        }


        /*
    public async Task<Users> GetById(int id)
    - Metode, der søger efter en bruger i databasen baseret på brugerens ID.

    return await _context.Users.FindAsync(id);
    - Bruger Entity Framework's FindAsync-metode til at finde en bruger med det angivne ID.
    - Returnerer null, hvis der ikke findes en bruger med det angivne ID.
*/
        public async Task<Users> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /*
    public async Task<IEnumerable<Users>> GetAll()
    - Metode, der henter alle brugere fra databasen.

    return await _context.Users.ToListAsync();
    - Bruger Entity Framework's ToListAsync-metode til at hente alle brugere som en liste.
*/
        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        /*
    public async Task<IEnumerable<Users>> GetDeletedUsers()
    - Metode, der henter alle slettede brugere fra databasen.

    return await _context.Users.Where(u => u.is_deleted).ToListAsync();
    - Bruger Entity Framework's Where-metode til at filtrere brugere baseret på is_deleted-ejendommen.
    - ToListAsync-metoden anvendes til at hente de filtrerede slettede brugere som en liste.
*/
        public async Task<IEnumerable<Users>> GetDeletedUsers()
        {
            return await _context.Users.Where(u => u.is_deleted).ToListAsync();
        }


        /*
    public async Task AddUser(Users user)
    - Metode, der tilføjer en ny bruger til databasen.

    _context.Users.Add(user);
    - Bruger Entity Framework's Add-metode til at tilføje den angivne bruger til konteksten.

    await _context.SaveChangesAsync();
    - Gemmer ændringerne i konteksten asynkront. Dette sikrer, at den nye bruger bliver gemt i databasen.
*/
        public async Task AddUser(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        /*
    public async Task UpdateUser(Users user)
    - Metode, der opdaterer en eksisterende bruger i databasen.

    _context.Entry(user).State = EntityState.Modified;
    - Markerer brugerobjektet som ændret i konteksten. Dette fortæller EF, at objektet skal opdateres i databasen.

    await _context.SaveChangesAsync();
    - Gemmer ændringerne i konteksten asynkront. Dette udløser en databaseopdatering, der opdaterer brugeroplysningerne i databasen.
*/
        public async Task UpdateUser(Users user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /*
    public async Task SoftDeleteUser(int id)
    - Metode, der udfører en soft delete af en bruger i databasen.

    var userToDelete = await _context.Users.FindAsync(id);
    - Finder brugeren, der skal slettes, ved hjælp af den angivne id.

    if (userToDelete != null)
    {
        userToDelete.is_deleted = true;
        - Sætter is_deleted-egenskaben til true, hvilket markerer brugeren som slettet.

        _context.Entry(userToDelete).State = EntityState.Modified;
        - Markerer brugerobjektet som ændret i konteksten, så ændringerne gemmes.

        await _context.SaveChangesAsync();
        - Gemmer ændringerne i konteksten asynkront, hvilket udløser en databaseopdatering.
    }
*/
        public async Task SoftDeleteUser(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete != null)
            {
                userToDelete.is_deleted = true;
                _context.Entry(userToDelete).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }

}