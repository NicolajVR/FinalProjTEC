using AutoMapper;
using skolesystem.DTOs;
using skolesystem.Models;
using skolesystem.Repository;

namespace skolesystem.Service
{
    /*
    En serviceklasse indeholder forretningslogik, der udfører specifikke opgaver og fungerer som et mellemled mellem
    brugergrænsefladen (UI) og databasen eller andre eksterne ressourcer. Den indeholder typisk metoder, der udfører
    operationer relateret til forretningsdomænet. I denne serviceklasse, der implementerer en interface, håndteres
    operationer relateret til fraværsdata, såsom oprettelse, opdatering, sletning og hentning af fravær.
    */


    // Definerer kontraktmetoder for en service, der håndterer forretningslogik relateret til fravær.
    // Grænsefladen specificerer metoder til at hente, oprette, opdatere og soft-slette fraværsdata.
    public interface IAbsenceService
    {
        // Henter fravær baseret på fraværs-id
        Task<AbsenceReadDto> GetAbsenceById(int id);

        // Henter alle fravær
        Task<IEnumerable<AbsenceReadDto>> GetAllAbsences();

        // Henter alle markerede slettede fravær
        Task<IEnumerable<AbsenceReadDto>> GetDeletedAbsences();

        // Opretter nyt fravær ved at modtage oplysninger fra en AbsenceCreateDto
        Task<AbsenceReadDto> CreateAbsence(AbsenceCreateDto absenceDto);

        // Opdaterer eksisterende fravær baseret på fraværs-id og oplysninger fra en AbsenceUpdateDto
        Task UpdateAbsence(int id, AbsenceUpdateDto absenceDto);

        // Markerer fravær som "soft-deleted" (ikke fysisk fjernet) baseret på fraværs-id
        Task SoftDeleteAbsence(int id);
    }

    // Implementerer IAbsenceService-grænsefladen og håndterer forretningslogik relateret til fravær.
    // Denne service giver metoder til at hente, oprette, opdatere og soft-slette fraværsdata.
    public class AbsenceService : IAbsenceService
    {
        private readonly IAbsenceRepository _absenceRepository; // Repository-objekt, der giver adgang til databasen for fraværsdata
        private readonly IMapper _mapper; // Mapper-objekt, der håndterer konvertering mellem dataobjekter og DTO'er

        public AbsenceService(IAbsenceRepository absenceRepository, IMapper mapper)
        {
            _absenceRepository = absenceRepository; // Initialiserer repository-objektet gennem dependency injection
            _mapper = mapper; // Initialiserer mapper-objektet gennem dependency injection
        }

        // Asynkron metode til at hente fravær baseret på fraværs-id og returnere en DTO (Data Transfer Object) repræsentation af fraværet.
        // Bruger asynkronitet for at undgå at blokere tråden under hentning af fraværsdata fra databasen.
        public async Task<AbsenceReadDto> GetAbsenceById(int id)
        {
            // Kalder repository-metoden for at hente fraværet fra databasen baseret på fraværs-id
            var absence = await _absenceRepository.GetById(id);

            // Hvis fraværet ikke eksisterer returnere null.
            if (absence == null)
            {
                return null; // Returnere null 
            }

            // Brug af mapper til at konvertere fravær til en DTO (AbsenceReadDto)
            var absenceDto = _mapper.Map<AbsenceReadDto>(absence);

            // Returnerer den konverterede DTO
            return absenceDto;
        }



        // Henter alle fravær fra databasen og konverterer dem til en IEnumerable af AbsenceReadDto.
        // Bruger IEnumerable, som tillader iteration over en samling af fraværsdata.
        public async Task<IEnumerable<AbsenceReadDto>> GetAllAbsences()
        {
            // Asynkront kalder repository-metoden for at hente alle fravær fra databasen
            var absences = await _absenceRepository.GetAll();

            // Brug af mapper til at konvertere fravær til en IEnumerable af AbsenceReadDto
            return _mapper.Map<IEnumerable<AbsenceReadDto>>(absences);
        }


        // Henter alle markerede slettede fravær fra databasen og konverterer dem til en IEnumerable af AbsenceReadDto.
        public async Task<IEnumerable<AbsenceReadDto>> GetDeletedAbsences()
        {
            // Asynkront kalder repository-metoden for at hente alle markerede slettede fravær fra databasen
            var deletedAbsences = await _absenceRepository.GetDeletedAbsences();

            // Brug af mapper til at konvertere fravær til en IEnumerable af AbsenceReadDto
            return _mapper.Map<IEnumerable<AbsenceReadDto>>(deletedAbsences);
        }


        // Opretter nyt fravær baseret på oplysninger fra AbsenceCreateDto og returnerer en DTO (Data Transfer Object) repræsentation af det oprettede fravær.
        public async Task<AbsenceReadDto> CreateAbsence(AbsenceCreateDto absenceDto)
        {
            // Opretter et nyt Absence-objekt med oplysningerne fra AbsenceCreateDto
            var absence = new Absence
            {
                user_id = absenceDto.user_id,
                teacher_id = absenceDto.teacher_id,
                class_id = absenceDto.class_id,
                absence_date = absenceDto.absence_date,
                reason = absenceDto.reason
            };

            // Asynkront kalder repository-metoden for at tilføje det oprettede fravær til databasen
            await _absenceRepository.AddAbsence(absence);

            // Mapper det oprettede Absence-objekt til en AbsenceReadDto, før det returneres
            var createdAbsenceDto = _mapper.Map<AbsenceReadDto>(absence);

            return createdAbsenceDto;
        }


        // Opdaterer eksisterende fravær baseret på fraværs-id og oplysninger fra AbsenceUpdateDto.
        public async Task UpdateAbsence(int id, AbsenceUpdateDto absenceDto)
        {
            // Asynkront henter det eksisterende fravær fra databasen baseret på fraværs-id
            var existingAbsence = await _absenceRepository.GetById(id);

            // Hvis fraværet ikke eksisterer, kast en ArgumentException
            if (existingAbsence == null)
            {
                throw new ArgumentException("Absence not found");
            }

            // Mapper oplysninger fra AbsenceUpdateDto til det eksisterende fravær
            _mapper.Map(absenceDto, existingAbsence);

            // Asynkront kalder repository-metoden for at opdatere det eksisterende fravær i databasen
            await _absenceRepository.UpdateAbsence(id, existingAbsence);
        }


        // Asynkron metode: Markerer fravær som "soft-deleted" (ikke fysisk fjernet) baseret på fraværs-id.
        public async Task SoftDeleteAbsence(int id)
        {
            // Asynkront henter det fravær, der skal markeres som "soft-deleted", fra databasen baseret på fraværs-id
            var absenceToDelete = await _absenceRepository.GetById(id);

            // Hvis fraværet ikke eksisterer, kast en ArgumentException
            if (absenceToDelete == null)
            {
                throw new ArgumentException("Absence not found");
            }

            // Asynkront kalder repository-metoden for at markere fraværet som "soft-deleted" i databasen
            await _absenceRepository.SoftDeleteAbsence(id);
        }

    }
}