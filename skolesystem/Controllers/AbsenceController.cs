using Microsoft.AspNetCore.Mvc;
using skolesystem.Authorization;
using skolesystem.DTOs;
using skolesystem.Service;

namespace skolesystem.Controllers
{

    /*
    
    - [Route("api/[controller]")]
      - Angiver den rutepræfiks, der bruges til at tilgå denne controller. 
      - I dette tilfælde er ruten "api/[controller]", hvor "[controller]" erstattes med controllerens navn ("AbsenceController").

    - [ApiController]
      - Markerer klassen som en ApiController, hvilket indebærer nogle konventioner og funktioner, der letter udviklingen af en web API-controller.

    - private readonly IAbsenceService _absenceService;
      - En privat medlemsvariabel, der indeholder en reference til den tjeneste, der håndterer fraværsrelaterede operationer.
      - Interfacet IAbsenceService bruges til at deklarere metoder til håndtering af fravær i servicelaget.

    - public AbsenceController(IAbsenceService absenceService)
      - En constructor, der tager en IAbsenceService som parameter.
      - Dependency Injection bruges til at levere en implementering af IAbsenceService til denne controller ved oprettelse.
      - Dette muliggør løs kobling og letter testning ved at erstatte den faktiske tjenesteimplementering med en testimplementering.

*/
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;

        public AbsenceController(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
        }


        /*
    Action-metoden GetAbsences i AbsenceController:

    - [HttpGet]
      - Angiver, at denne metode skal reagere på HTTP GET-anmodninger.
      - Dette betyder, at den bruges til at hente data og sende det som svar.

    - public async Task<IEnumerable<AbsenceReadDto>> GetAbsences()
      - Returnerer en asynkron opgave, der repræsenterer en samling af fraværsobjekter i form af AbsenceReadDto.

    - var absences = await _absenceService.GetAllAbsences();
      - Kalder metoden GetAllAbsences på _absenceService, som returnerer en samling af fraværsobjekter.

    - var absenceDtos = new List<AbsenceReadDto>();
      - Opretter en tom liste af AbsenceReadDto, der vil indeholde de DTO'er, der skal sendes som svar.

    - foreach (var absence in absences)
      - Itererer gennem hver Absence i den modtagne samling af fravær.

    - absenceDtos.Add(new AbsenceReadDto { ... });
      - For hver Absence konverteres den til en AbsenceReadDto og tilføjes til absenceDtos-listen.
      - Dette er en form for mapping mellem dine modelobjekter og DTO'er.

    - return absenceDtos;
      - Returnerer listen af AbsenceReadDto som svar på anmodningen.

    - Denne metode henter fraværsdata fra tjenesten, mapper dem til DTO'er og sender dem som JSON-respons.
    - Ved hjælp af asynkrone metoder sikrer den, at anmodningen ikke blokerer tråden, mens data hentes fra tjenesten

*/      [Authorize(2)]
        [HttpGet]
        public async Task<IEnumerable<AbsenceReadDto>> GetAbsences()
        {
            var absences = await _absenceService.GetAllAbsences();
            var absenceDtos = new List<AbsenceReadDto>();
            foreach (var absence in absences)
            {
                absenceDtos.Add(new AbsenceReadDto
                {
                    absence_id = absence.absence_id,
                    user_id = absence.user_id,
                    teacher_id = absence.teacher_id,
                    class_id = absence.class_id,
                    absence_date = absence.absence_date,
                    reason = absence.reason,
                    is_deleted = absence.is_deleted
                });
            }
            return absenceDtos;
        }

        /*
    Action-metoden GetAbsenceById i AbsenceController:

    - [HttpGet("{id}")]
      - Angiver, at denne metode skal reagere på HTTP GET-anmodninger, hvor "id" er en variabel i stien.
      - Dette betyder, at den bruges til at hente specifikke fraværsdata baseret på det angivne ID.

    - [ProducesResponseType(typeof(AbsenceReadDto), StatusCodes.Status200OK)]
      - Angiver de forventede svartyper og statuskoder for en succesfuld anmodning.
      - Status200OK angiver, at anmodningen var succesfuld, og responsen vil være af typen AbsenceReadDto.

    - [ProducesResponseType(StatusCodes.Status404NotFound)]
      - Angiver, at hvis ressourcen ikke findes, forventes en statuskode 404 (Not Found) som svar.

    - public async Task<IActionResult> GetAbsenceById(int id)
      - Returnerer en IActionResult, der repræsenterer resultatet af anmodningen.

    - var absence = await _absenceService.GetAbsenceById(id);
      - Kalder metoden GetAbsenceById på _absenceService for at hente fraværsdata baseret på det angivne ID.

    - if (absence == null)
      - Hvis fraværet ikke findes, returneres en NotFound-respons (statuskode 404).

    - var absenceDto = new AbsenceReadDto { ... };
      - Hvis fraværet findes, oprettes en AbsenceReadDto ved at mappe data fra Absence-objektet.

    - return Ok(absenceDto);
      - Returnerer en succesfuld respons (statuskode 200 OK) sammen med AbsenceReadDto som JSON-respons.

    - NotFound() returnerer en HTTP 404-respons, hvis ressourcen ikke findes.
    - Ok(absenceDto) returnerer en HTTP 200-respons med AbsenceReadDto som svar, hvis fraværet findes.

*/
        [Authorize(2)]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AbsenceReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAbsenceById(int id)
        {
            var absence = await _absenceService.GetAbsenceById(id);

            if (absence == null)
            {
                return NotFound();
            }

            var absenceDto = new AbsenceReadDto
            {
                absence_id = absence.absence_id,
                user_id = absence.user_id,
                teacher_id = absence.teacher_id,
                class_id = absence.class_id,
                absence_date = absence.absence_date,
                reason = absence.reason,
                is_deleted = absence.is_deleted
            };

            return Ok(absenceDto);
        }

        /*
    Action-metoden CreateAbsence i AbsenceController:

    - [HttpPost]
      - Angiver, at denne metode skal reagere på HTTP POST-anmodninger.
      - Dette betyder, at den bruges til at oprette ny fraværsdata ved at modtage data i anmodningens krop.

    - [ProducesResponseType(StatusCodes.Status201Created)]
      - Angiver, at en vellykket oprettelsesanmodning skal returnere statuskoden 201 Created.

    - public async Task<IActionResult> CreateAbsence(AbsenceCreateDto absenceDto)
      - Returnerer en IActionResult, der repræsenterer resultatet af oprettelsesanmodningen.

    - var createdAbsenceDto = await _absenceService.CreateAbsence(absenceDto);
      - Kalder metoden CreateAbsence på _absenceService for at oprette fraværsdata baseret på de modtagne oplysninger.

    - return CreatedAtAction(nameof(GetAbsenceById), new { id = createdAbsenceDto.absence_id }, createdAbsenceDto);
      - Returnerer en HTTP 201 Created-respons sammen med en URL til den oprettede ressource.
      - CreatedAtAction bruges til at oprette et svar med status 201 og en Location-header, der indeholder URL'en til den nyoprettede ressource.
      - nameof(GetAbsenceById) angiver handlingen, der bruges til at hente den oprettede ressource.
      - { id = createdAbsenceDto.absence_id } er den ruteværdi, der bruges til at identificere den oprettede ressource.

    - ProducesResponseType-attributten dokumenterer forventet statuskode for en vellykket anmodning.
    - CreatedAtAction bruges til at generere en korrekt Location-header for den oprettede ressource.

*/      [Authorize(2)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAbsence(AbsenceCreateDto absenceDto)
        {
            // Call the service method to create the absence
            var createdAbsenceDto = await _absenceService.CreateAbsence(absenceDto);

            // Return the created AbsenceReadDto in the response
            return CreatedAtAction(nameof(GetAbsenceById), new { id = createdAbsenceDto.absence_id }, createdAbsenceDto);
        }



        /*
    Action-metoden UpdateAbsence i AbsenceController:

    - [HttpPut("{id}")]
      - Angiver, at denne metode skal reagere på HTTP PUT-anmodninger, hvor "id" er en variabel i stien.
      - Dette betyder, at den bruges til at opdatere eksisterende fraværsdata baseret på det angivne ID.

    - public async Task<IActionResult> UpdateAbsence(int id, AbsenceUpdateDto absenceDto)
      - Returnerer en IActionResult, der repræsenterer resultatet af opdateringsanmodningen.

    - var existingAbsence = await _absenceService.GetAbsenceById(id);
      - Kalder metoden GetAbsenceById på _absenceService for at hente eksisterende fraværsdata baseret på det angivne ID.

    - if (existingAbsence == null)
      - Hvis fraværet ikke findes, returneres en NotFound-respons (statuskode 404).

    - await _absenceService.UpdateAbsence(id, absenceDto);
      - Hvis fraværet findes, opdateres det ved at kalde UpdateAbsence-metoden på _absenceService og give de nye oplysninger.

    - return NoContent();
      - Returnerer en HTTP 204 No Content-respons, hvilket indikerer, at opdateringen blev udført succesfuldt, men der er intet nyt indhold at sende.

    - NotFound() returnerer en HTTP 404-respons, hvis ressourcen ikke findes.
    - NoContent() returnerer en HTTP 204-respons for en vellykket, men tom opdatering.

*/      [Authorize(2)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbsence(int id, AbsenceUpdateDto absenceDto)
        {
            var existingAbsence = await _absenceService.GetAbsenceById(id);

            if (existingAbsence == null)
            {
                return NotFound();
            }

            // Pass absenceDto directly to the UpdateAbsence method
            await _absenceService.UpdateAbsence(id, absenceDto);

            return NoContent();
        }



        /*
    Action-metoden DeleteAbsence i AbsenceController:

    - [HttpDelete("{id}")]
      - Angiver, at denne metode skal reagere på HTTP DELETE-anmodninger, hvor "id" er en variabel i stien.
      - Dette betyder, at den bruges til at slette eksisterende fraværsdata baseret på det angivne ID.

    - [ProducesResponseType(StatusCodes.Status204NoContent)]
      - Angiver, at en vellykket sletningsanmodning skal returnere statuskoden 204 No Content.

    - [ProducesResponseType(StatusCodes.Status404NotFound)]
      - Angiver, at hvis ressourcen ikke findes, forventes en statuskode 404 (Not Found) som svar.

    - public async Task<IActionResult> DeleteAbsence(int id)
      - Returnerer en IActionResult, der repræsenterer resultatet af sletningsanmodningen.

    - var absenceToDelete = await _absenceService.GetAbsenceById(id);
      - Kalder metoden GetAbsenceById på _absenceService for at hente fraværsdata baseret på det angivne ID.

    - if (absenceToDelete == null)
      - Hvis fraværet ikke findes, returneres en NotFound-respons (statuskode 404).

    - await _absenceService.SoftDeleteAbsence(id);
      - Hvis fraværet findes, bliver det markeret som slettet ved at kalde SoftDeleteAbsence-metoden på _absenceService.

    - return NoContent();
      - Returnerer en HTTP 204 No Content-respons, hvilket indikerer, at sletningen blev udført succesfuldt, men der er intet nyt indhold at sende.

    - NotFound() returnerer en HTTP 404-respons, hvis ressourcen ikke findes.
    - NoContent() returnerer en HTTP 204-respons for en vellykket, men tom sletning.

*/      [Authorize(2)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAbsence(int id)
        {
            var absenceToDelete = await _absenceService.GetAbsenceById(id);

            if (absenceToDelete == null)
            {
                return NotFound();
            }

            await _absenceService.SoftDeleteAbsence(id);

            return NoContent();
        }
    }
}