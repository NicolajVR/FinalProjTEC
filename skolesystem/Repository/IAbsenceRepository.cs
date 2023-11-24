using AutoMapper;
using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.Models;

namespace skolesystem.Repository
{
    /*
    Et repository-grænseflade definerer kontraktmetoder, der giver adgang til persistente datakilder, typisk en database.
    Her fokuseres grænsefladen på operationer relateret til fraværsdata.

    GetById:
    - Ansvarlig for at hente et fravær fra databasen baseret på det unikke fraværs-id.

    GetAll:
    - Ansvarlig for at hente alle fravær fra databasen.

    GetDeletedAbsences:
    - Ansvarlig for at hente alle markerede slettede fravær fra databasen.

    AddAbsence:
    - Ansvarlig for at tilføje et nyt fravær til databasen.

    UpdateAbsence:
    - Ansvarlig for at opdatere et eksisterende fravær i databasen baseret på fraværs-id.

    SoftDeleteAbsence:
    - Ansvarlig for at markere et fravær som "soft-deleted" i stedet for at fjerne det fysisk fra databasen.

    Hver metode beskriver en specifik opgave inden for håndtering af fraværsdata i databasen. Implementeringen af denne
    grænseflade skal håndtere kommunikationen med den underliggende datakilde og udføre de nødvendige databaseoperationer.
*/
    public interface IAbsenceRepository
    {
        Task<Absence> GetById(int id);
        Task<IEnumerable<Absence>> GetAll();
        Task<IEnumerable<Absence>> GetDeletedAbsences();
        Task AddAbsence(Absence absence);
        Task UpdateAbsence(int id, Absence absence);
        Task SoftDeleteAbsence(int id);
    }

    /*
    En repository-implementation, der håndterer adgang til og manipulation af fraværsdata i en database. Den implementerer
    IAbsenceRepository-grænsefladen og bruger en DbContext til at kommunikere med databasen.

    Constructor:
    - Modtager en AbsenceDbContext og en IMapper gennem afhængighedsinjektion for at få adgang til og manipulere databasen.


    - Håndterer kommunikationen med databasen ved at implementere metoder fra IAbsenceRepository-grænsefladen.
    - Mapper mellem databaseentiteter og DTO'er ved hjælp af IMapper.

    - Denne klasse håndtere databaseinteraktion og implementere logikken for at udføre de nødvendige operationer på
      fraværsdata.
*/
    public class AbsenceRepository : IAbsenceRepository
    {
        private readonly AbsenceDbContext _context; // DbContext til databasen
        private readonly IMapper _mapper; // Mapper til konvertering mellem databaseentiteter og DTO'er

        public AbsenceRepository(AbsenceDbContext context, IMapper mapper)
        {
            _context = context; // Initialiserer DbContext gennem afhængighedsinjektion
            _mapper = mapper; // Initialiserer mapper-objektet gennem afhængighedsinjektion
        }



        /*
    Asynkron metode: Henter et fravær fra databasen baseret på det unikke fraværs-id.

    Parameters:
    - id: Det unikke identifikationsnummer for det ønskede fravær.

    Returnerer:
    - En Task, der repræsenterer det fundne fravær eller null, hvis fraværet ikke findes i databasen.

    Bemærk:
    - Denne metode bruger Entity Frameworks FindAsync-metode til at asynkront søge efter fraværet i databasen.
    - Hvis fraværet ikke findes, vil FindAsync returnere null.
*/
        public async Task<Absence> GetById(int id)
        {
            return await _context.Absence.FindAsync(id);
        }



        /*
    Asynkron metode: Henter alle fravær fra databasen.

    Returnerer:
    - En Task, der repræsenterer en IEnumerable af fravær fra databasen.

    Bemærk:
    - Denne metode bruger Entity Frameworks ToListAsync-metode til at asynkront hente alle fravær fra databasen.
*/
        public async Task<IEnumerable<Absence>> GetAll()
        {
            return await _context.Absence.ToListAsync();
        }



        /*
    Asynkron metode: Henter alle markerede slettede fravær fra databasen.

    Returnerer:
    - En Task, der repræsenterer en IEnumerable af markerede slettede fravær fra databasen.

    Bemærk:
    - Denne metode bruger Entity Frameworks Where-klausul til at filtrere fraværsdata baseret på is_deleted-flaget.
    - Bruger ToListAsync-metoden til at asynkront hente resultatet som en liste.
*/
        public async Task<IEnumerable<Absence>> GetDeletedAbsences()
        {
            return await _context.Absence.Where(a => a.is_deleted).ToListAsync();
        }


        /*
    Asynkron metode: Tilføjer et nyt fravær til databasen.

    Parameters:
    - absence: Det fravær, der skal tilføjes til databasen.

    Bemærk:
    - Tilføjer det angivne fravær til Absence DbSet i DbContext (_context).
    - Asynkront kalder SaveChangesAsync for at gemme ændringerne i databasen.
*/
        public async Task AddAbsence(Absence absence)
        {
            _context.Absence.Add(absence);
            await _context.SaveChangesAsync();
        }




        /*
    Asynkron metode: Opdaterer et eksisterende fravær i databasen baseret på fraværs-id.

    Parameters:
    - id: Det unikke identifikationsnummer for det fravær, der skal opdateres.
    - absence: Det fravær med opdaterede oplysninger.

    Bemærk:
    - Asynkront henter det eksisterende fravær fra databasen baseret på det angivne fraværs-id.
    - Hvis fraværet ikke findes, kastes en ArgumentException.
    - Opdaterer oplysningerne i det eksisterende fravær med oplysningerne fra det angivne fravær.
    - Asynkront kalder SaveChangesAsync for at gemme ændringerne i databasen.
*/
        public async Task UpdateAbsence(int id, Absence absence)
        {
            // Henter det eksisterende fravær fra databasen baseret på fraværs-id
            var existingAbsence = await _context.Absence.FindAsync(id);

            // Hvis fraværet ikke eksisterer, kast en ArgumentException
            if (existingAbsence == null)
            {
                throw new ArgumentException("Absence not found");
            }

            // Opdaterer oplysningerne i det eksisterende fravær med oplysningerne fra det angivne fravær
            existingAbsence.user_id = absence.user_id;
            existingAbsence.teacher_id = absence.teacher_id;
            existingAbsence.class_id = absence.class_id;
            existingAbsence.absence_date = absence.absence_date;
            existingAbsence.reason = absence.reason;

            // Asynkront kalder SaveChangesAsync for at gemme ændringerne i databasen
            await _context.SaveChangesAsync();
        }


        /*
    Asynkron metode: Markerer et fravær som "soft-deleted" (ikke fysisk fjernet) i databasen baseret på fraværs-id.

    Parameters:
    - id: Det unikke identifikationsnummer for det fravær, der skal markeres som "soft-deleted".

    Bemærk:
    - Asynkront henter det fravær, der skal markeres som "soft-deleted", fra databasen baseret på det angivne fraværs-id.
    - Hvis fraværet eksisterer, markeres det som "soft-deleted" ved at ændre is_deleted-flaget til true.
    - EntityState.Modified fortæller DbContext, at objektet er blevet ændret, og disse ændringer skal gemmes.
    - Asynkront kalder SaveChangesAsync for at gemme ændringerne i databasen.
*/
        public async Task SoftDeleteAbsence(int id)
        {
            // Henter det fravær, der skal markeres som "soft-deleted", fra databasen baseret på fraværs-id
            var absenceToDelete = await _context.Absence.FindAsync(id);

            // Hvis fraværet eksisterer, markeres det som "soft-deleted" ved at ændre is_deleted-flaget til true
            if (absenceToDelete != null)
            {
                absenceToDelete.is_deleted = true;

                // EntityState.Modified fortæller DbContext, at objektet er blevet ændret, og disse ændringer skal gemmes
                _context.Entry(absenceToDelete).State = EntityState.Modified;

                // Asynkront kalder SaveChangesAsync for at gemme ændringerne i databasen
                await _context.SaveChangesAsync();
            }
        }

    }
}