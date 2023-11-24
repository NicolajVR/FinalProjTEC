using skolesystem.DTOs;
using skolesystem.Models;


/*
    public interface IScheduleService
    - Interface, der specificerer metoder til håndtering af schedule-operationer.

    Task<IEnumerable<Schedule>> GetAllSchemata();
    - Returnerer en opgave, der repræsenterer en samling af alle schedules.

    Task<Schedule> GetScheduleById(int id);
    - Returnerer en opgave, der repræsenterer en schedule baseret på det angivne id.

    Task<int> CreateSchedule(Schedule schedule);
    - Returnerer en opgave, der repræsenterer oprettelsen af en ny schedule, og returnerer id'et for den oprettede schedule.

    Task UpdateSchedule(int id, ScheduleCreateDto scheduleDto);
    - Returnerer en opgave, der repræsenterer opdateringen af en eksisterende schedule baseret på id'et og de nye oplysninger fra ScheduleCreateDto.

    Task DeleteSchedule(int id);
    - Returnerer en opgave, der repræsenterer sletningen af en schedule baseret på det angivne id.
*/
public interface IScheduleService
{
    Task<IEnumerable<Schedule>> GetAllSchemata();
    Task<Schedule> GetScheduleById(int id);
    Task<int> CreateSchedule(Schedule schedule);
    Task UpdateSchedule(int id, ScheduleCreateDto scheduleDto);
    Task DeleteSchedule(int id);
}


/*
    public class ScheduleService : IScheduleService
    - Implementering af IScheduleService-interfacet.

    private readonly IScheduleRepository _scheduleRepository;
    - Privat variabel til opbevaring af en reference til en instans af IScheduleRepository.

    public ScheduleService(IScheduleRepository scheduleRepository)
    - Konstruktør, der tager en IScheduleRepository som parameter og initialiserer _scheduleRepository med den angivne instans.

*/
public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;

    public ScheduleService(IScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }


    /*
    public async Task<IEnumerable<Schedule>> GetAllSchemata()
    - Implementering af GetAllSchemata-metoden fra IScheduleService-interfacet.

    return await _scheduleRepository.GetAll();
    - Kalder GetAll-metoden på _scheduleRepository og returnerer resultatet.
      Dette indebærer at hente alle oplysninger om skemaer fra datakilden, f.eks. en database.

*/
    public async Task<IEnumerable<Schedule>> GetAllSchemata()
    {
        return await _scheduleRepository.GetAll();
    }


    /*
    public async Task<Schedule> GetScheduleById(int id)
    - Implementering af GetScheduleById-metoden fra IScheduleService-interfacet.

    return await _scheduleRepository.GetById(id);
    - Kalder GetById-metoden på _scheduleRepository med det angivne id og returnerer resultatet.
      Dette indebærer at hente oplysninger om et skema med det specificerede id fra datakilden, f.eks. en database.

*/
    public async Task<Schedule> GetScheduleById(int id)
    {
        return await _scheduleRepository.GetById(id);
    }

    /*
    public async Task<int> CreateSchedule(Schedule schedule)
    - Implementering af CreateSchedule-metoden fra IScheduleService-interfacet.

    return await _scheduleRepository.Create(schedule);
    - Kalder Create-metoden på _scheduleRepository med det angivne skemaobjekt og returnerer resultatet.
      Dette indebærer at oprette et nyt skema i datakilden, f.eks. i en database, og returnere det tildelte id.

*/
    public async Task<int> CreateSchedule(Schedule schedule)
    {
        return await _scheduleRepository.Create(schedule);
    }


    /*
    public async Task UpdateSchedule(int id, ScheduleCreateDto scheduleDto)
    - Implementering af UpdateSchedule-metoden fra IScheduleService-interfacet.

    var existingSchedule = await _scheduleRepository.GetById(id);
    - Henter det eksisterende skema fra datakilden ved hjælp af GetById-metoden på _scheduleRepository.

    if (existingSchedule == null)
    {
        throw new ArgumentException($"Schedule with ID {id} not found.");
    }
    - Tjekker om det eksisterende skema eksisterer. Hvis ikke, kastes en ArgumentException med en fejlmeddelelse.

    await _scheduleRepository.Update(id, scheduleDto);
    - Kalder Update-metoden på _scheduleRepository med ID'et og opdaterede oplysninger fra scheduleDto.
      Dette indebærer sandsynligvis opdatering af skemaet i datakilden, f.eks. i en database.
*/
    public async Task UpdateSchedule(int id, ScheduleCreateDto scheduleDto)
    {
        var existingSchedule = await _scheduleRepository.GetById(id);

        if (existingSchedule == null)
        {
            throw new ArgumentException($"Schedule with ID {id} not found.");
        }

        await _scheduleRepository.Update(id, scheduleDto);
    }

    /*
    public async Task DeleteSchedule(int id)
    - Implementering af DeleteSchedule-metoden fra IScheduleService-interfacet.

    await _scheduleRepository.Delete(id);
    - Kalder Delete-metoden på _scheduleRepository med ID'et.
      Dette indebærer sletning af skemaet fra datakilden, f.eks. fra en database.
*/
    public async Task DeleteSchedule(int id)
    {
        await _scheduleRepository.Delete(id);
    }
}