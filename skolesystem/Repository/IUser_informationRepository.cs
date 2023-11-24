using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.Models;

/*
    public interface IUser_informationRepository
    - Interface, der definerer metoder til databehandling af User_information-entiteter.

    Task<User_information> GetById(int id);
    - Henter en User_information-entitet baseret på det angivne id.

    Task<IEnumerable<User_information>> GetAll();
    - Henter alle User_information-entiteter.

    Task<IEnumerable<User_information>> GetDeletedUser_informations();
    - Henter alle slettede User_information-entiteter.

    Task AddUser_information(User_information user_information);
    - Tilføjer en ny User_information-entitet til databasen.

    Task UpdateUser_information(int id, User_information updatedUser_information);
    - Opdaterer en eksisterende User_information-entitet baseret på det angivne id.

    Task SoftDeleteUser_information(int id);
    - Markerer en User_information-entitet som slettet (soft delete) baseret på det angivne id.
*/
public interface IUser_informationRepository
{
    Task<User_information> GetById(int id);
    Task<IEnumerable<User_information>> GetAll();
    Task<IEnumerable<User_information>> GetDeletedUser_informations();
    Task AddUser_information(User_information user_information);
    Task UpdateUser_information(int id, User_information updatedUser_information);
    Task SoftDeleteUser_information(int id);
}

/*
    public class User_informationRepository : IUser_informationRepository
    - Implementering af IUser_informationRepository-interface.

    private readonly User_informationDbContext _context;
    - Private variabel til at gemme en instans af User_informationDbContext, der giver adgang til databasen.

    public User_informationRepository(User_informationDbContext context)
    {
        _context = context;
    }
    - Konstruktør, der modtager en User_informationDbContext-injektion og gemmer den i _context-variablen.
*/
public class User_informationRepository : IUser_informationRepository
{
    private readonly User_informationDbContext _context;

    public User_informationRepository(User_informationDbContext context)
    {
        _context = context;
    }

    /*
    public async Task<User_information> GetById(int id)
    - Metode til at hente en User_information fra databasen baseret på det angivne id.

    return await _context.User_information.FindAsync(id);
    - Returnerer resultatet af at finde en User_information i databasen ved hjælp af det angivne id.
*/
    public async Task<User_information> GetById(int id)
    {
        return await _context.User_information.FindAsync(id);
    }

    /*
    public async Task<IEnumerable<User_information>> GetAll()
    - Metode til at hente alle User_information-objekter fra databasen.

    return await _context.User_information.ToListAsync();
    - Returnerer resultatet af at hente alle User_information-objekter fra databasen som en liste.
*/
    public async Task<IEnumerable<User_information>> GetAll()
    {
        return await _context.User_information.ToListAsync();
    }

    /*
    public async Task<IEnumerable<User_information>> GetDeletedUser_informations()
    - Metode til at hente alle slettede User_information-objekter fra databasen.

    return await _context.User_information.Where(b => b.is_deleted).ToListAsync();
    - Returnerer resultatet af at filtrere User_information-objekterne baseret på is_deleted-ejendommen (hvor den er sand) og konvertere dem til en liste.
*/
    public async Task<IEnumerable<User_information>> GetDeletedUser_informations()
    {
        return await _context.User_information.Where(b => b.is_deleted).ToListAsync();
    }


    /*
    public async Task AddUser_information(User_information user_information)
    - Metode til at tilføje et User_information-objekt til databasen.

    _context.User_information.Add(user_information);
    - Tilføjer det givne User_information-objekt til DbSet<User_information> i den tilknyttede DbContext.

    await _context.SaveChangesAsync();
    - Gemmer ændringerne i databasen og udfører de nødvendige INSERT SQL-kommandoer for at tilføje det nye User_information-objekt.
*/
    public async Task AddUser_information(User_information user_information)
    {
        _context.User_information.Add(user_information);
        await _context.SaveChangesAsync();
    }


    /*
    public async Task UpdateUser_information(int id, User_information updatedUser_information)
    - Metode til at opdatere et eksisterende User_information-objekt i databasen.

    var existingUser_information = await _context.User_information.FindAsync(id);
    - Finder det eksisterende User_information-objekt i databasen baseret på den angivne id.

    if (existingUser_information == null)
    {
        throw new ArgumentException("Absence not found");
    }
    - Hvis det eksisterende User_information-objekt ikke findes, kastes en ArgumentException.

    // Update properties of existingUser_information with updatedUser_information
    - Opdaterer egenskaberne af det eksisterende User_information-objekt med værdierne fra updatedUser_information.

    await _context.SaveChangesAsync();
    - Gemmer ændringerne i databasen og udfører de nødvendige UPDATE SQL-kommandoer for at opdatere det eksisterende User_information-objekt.
*/
    public async Task UpdateUser_information(int id, User_information updatedUser_information)
    {
        var existingUser_information = await _context.User_information.FindAsync(id);


        if (existingUser_information == null)
        {
            throw new ArgumentException("Absence not found");
        }
        // Update properties of existingUser_information with updatedUser_information
        existingUser_information.user_information_id = updatedUser_information.user_information_id;
        existingUser_information.name = updatedUser_information.name;
        existingUser_information.last_name = updatedUser_information.last_name;
        existingUser_information.phone = updatedUser_information.phone;
        existingUser_information.date_of_birth = updatedUser_information.date_of_birth;
        existingUser_information.address = updatedUser_information.address;
        existingUser_information.is_deleted = updatedUser_information.is_deleted;
        existingUser_information.gender_id = updatedUser_information.gender_id;

        await _context.SaveChangesAsync();
    }


    /*
    public async Task SoftDeleteUser_information(int id)
    - Metode til at udføre en "soft delete" af et User_information-objekt i databasen.

    var user_informationToDelete = await _context.User_information.FindAsync(id);
    - Finder det User_information-objekt, der skal slettes, baseret på den angivne id.

    if (user_informationToDelete != null)
    {
        user_informationToDelete.is_deleted = true;
        - Markerer User_information-objektet som slettet ved at sætte is_deleted-egenskaben til true.

        await _context.SaveChangesAsync();
        - Gemmer ændringerne i databasen og udfører de nødvendige UPDATE SQL-kommandoer for at opdatere is_deleted-egenskaben.
    }
*/
    public async Task SoftDeleteUser_information(int id)
    {
        var user_informationToDelete = await _context.User_information.FindAsync(id);

        if (user_informationToDelete != null)
        {
            user_informationToDelete.is_deleted = true;
            await _context.SaveChangesAsync();
        }
    }
}