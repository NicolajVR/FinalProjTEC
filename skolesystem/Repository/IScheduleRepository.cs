using Microsoft.EntityFrameworkCore;
using skolesystem.Data;
using skolesystem.DTOs;
using skolesystem.Models;


/*
    public interface IScheduleRepository
    - Definition af et interface, der specificerer metoder til databehandling af schedules.

    Task<IEnumerable<Schedule>> GetAll();
    - Metode til at hente alle schedules fra databasen.

    Task<Schedule> GetById(int id);
    - Metode til at hente en specifik schedule ud fra dens ID.

    Task<int> Create(Schedule schedule);
    - Metode til at oprette en ny schedule i databasen og returnere dens ID.

    Task Update(int id, ScheduleCreateDto scheduleDto);
    - Metode til at opdatere en eksisterende schedule i databasen baseret på ID og oplysninger fra en ScheduleCreateDto.

    Task Delete(int id);
    - Metode til at slette en schedule fra databasen baseret på ID.
*/
public interface IScheduleRepository
{
    Task<IEnumerable<Schedule>> GetAll();
    Task<Schedule> GetById(int id);
    Task<int> Create(Schedule schedule);
    Task Update(int id, ScheduleCreateDto scheduleDto);
    Task Delete(int id);
}

/*
    public class ScheduleRepository : IScheduleRepository
    - Implementering af IScheduleRepository-interfacet.

    private readonly ScheduleDbContext _context;
    - Privat felt til at holde en reference til ScheduleDbContext, der bruges til at arbejde med databasen.

    public ScheduleRepository(ScheduleDbContext context)
    {
        _context = context;
    }
    - Konstruktør, der tager en ScheduleDbContext som parameter og gemmer den i det private felt.
*/
public class ScheduleRepository : IScheduleRepository
{
    private readonly ScheduleDbContext _context;

    public ScheduleRepository(ScheduleDbContext context)
    {
        _context = context;
    }


    /*
    public async Task<IEnumerable<Schedule>> GetAll()
    - Implementering af GetAll-metoden fra IScheduleRepository.

    return await _context.Schedule.ToListAsync();
    - Returnerer alle schedules fra databasen som en liste ved at bruge Entity Frameworks ToListAsync-metode.
*/
    public async Task<IEnumerable<Schedule>> GetAll()
    {
        return await _context.Schedule.ToListAsync();
    }

    /*
    public async Task<Schedule> GetById(int id)
    - Implementering af GetById-metoden fra IScheduleRepository.

    return await _context.Schedule.FindAsync(id);
    - Finder og returnerer et schedule i databasen med den angivne id ved at bruge Entity Frameworks FindAsync-metode.
*/
    public async Task<Schedule> GetById(int id)
    {
        return await _context.Schedule.FindAsync(id);
    }


    /*
    public async Task<int> Create(Schedule schedule)
    - Implementering af Create-metoden fra IScheduleRepository.

    _context.Schedule.Add(schedule);
    - Tilføjer det givne schedule-objekt til Schedule-databasen.

    await _context.SaveChangesAsync();
    - Gemmer ændringerne i databasen.

    return schedule.schedule_id;
    - Returnerer id'et for det oprettede schedule.
*/
    public async Task<int> Create(Schedule schedule)
    {
        _context.Schedule.Add(schedule);
        await _context.SaveChangesAsync();
        return schedule.schedule_id;
    }

    /*
    public async Task Update(int id, ScheduleCreateDto scheduleDto)
    - Implementering af Update-metoden fra IScheduleRepository.

    var scheduleToUpdate = await _context.Schedule.FindAsync(id);
    - Finder det schedule, der skal opdateres, ved hjælp af det angivne id.

    if (scheduleToUpdate == null)
    {
        throw new ArgumentException("Schedule not found");
    }
    - Hvis det ønskede schedule ikke findes, kastes en ArgumentException.

    // Map properties from DTO to the entity
    scheduleToUpdate.subject_id = scheduleDto.subject_id;
    scheduleToUpdate.day_of_week = scheduleDto.day_of_week;
    scheduleToUpdate.end_time = scheduleDto.end_time;
    scheduleToUpdate.class_id = scheduleDto.class_id;
    - Opdaterer egenskaberne for schedule med værdierne fra ScheduleCreateDto.

    await _context.SaveChangesAsync();
    - Gemmer ændringerne i databasen.
*/
    public async Task Update(int id, ScheduleCreateDto scheduleDto)
    {
        var scheduleToUpdate = await _context.Schedule.FindAsync(id);

        if (scheduleToUpdate == null)
        {
            throw new ArgumentException("Schedule not found");
        }

        // Map properties from DTO to the entity
        scheduleToUpdate.subject_id = scheduleDto.subject_id;
        scheduleToUpdate.day_of_week = scheduleDto.day_of_week;
        scheduleToUpdate.end_time = scheduleDto.end_time;
        scheduleToUpdate.class_id = scheduleDto.class_id;

        await _context.SaveChangesAsync();
    }


    /*
    public async Task Delete(int id)
    - Implementering af Delete-metoden fra IScheduleRepository.

    var scheduleToDelete = await _context.Schedule.FindAsync(id);
    - Finder det schedule, der skal slettes, ved hjælp af det angivne id.

    if (scheduleToDelete == null)
    {
        throw new ArgumentException("Schedule not found");
    }
    - Hvis det ønskede schedule ikke findes, kastes en ArgumentException.

    _context.Schedule.Remove(scheduleToDelete);
    - Fjerner det fundne schedule fra DbContext.

    await _context.SaveChangesAsync();
    - Gemmer ændringerne i databasen.
*/
    public async Task Delete(int id)
    {
        var scheduleToDelete = await _context.Schedule.FindAsync(id);

        if (scheduleToDelete == null)
        {
            throw new ArgumentException("Schedule not found");
        }

        _context.Schedule.Remove(scheduleToDelete);
        await _context.SaveChangesAsync();
    }

}