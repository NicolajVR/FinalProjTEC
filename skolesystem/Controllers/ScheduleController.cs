using Microsoft.AspNetCore.Mvc;
using skolesystem.DTOs;
using skolesystem.Models;

namespace skolesystem.Controllers
{


    /*
    ScheduleController:

    - [Route("api/[controller]")]
      - Angiver routen for API-controlleren. I dette tilfælde er ruten "/api/schedule".

    - [ApiController]
      - Indikerer, at dette er en API-controller og aktiverer nogle konventioner for API-adfærd.

    - public class ScheduleController : ControllerBase
      - Definerer ScheduleController-klassen, der fungerer som en controller til at håndtere HTTP-anmodninger vedrørende schedules (tidsplaner).

    - private readonly IScheduleRepository _scheduleRepository;
      - Privat felt til at gemme en instans af IScheduleRepository. Repository'en bruges til at udføre operationer på Schedule-entiteten i databasen.

    - public ScheduleController(IScheduleRepository scheduleRepository)
      - ScheduleController's konstruktør, der modtager en instans af IScheduleRepository gennem afhængighedsinjektion.
      - Dette muliggør adgang til repository'en i controllerens metoder.

    - Denne controller håndterer HTTP-anmodninger relateret til schedules, og IScheduleRepository bruges til at udføre databaseoperationer på Schedule-entiteten.
*/
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }



        /*
    HTTP GET-metode i ScheduleController:

    - [HttpGet]
      - Angiver, at dette er en HTTP GET-metode, der svarer på anmodninger om at hente data.

    - public async Task<IActionResult> Get()
      - Asynkron metode, der håndterer HTTP GET-anmodninger til at hente alle schedules.

    - var scheduleList = await _scheduleRepository.GetAll();
      - Kalder GetAll-metoden på IScheduleRepository for at hente en liste over alle schedules fra databasen.

    - return Ok(scheduleList);
      - Returnerer et HTTP 200 OK-svar med listen over schedules som responskrop, hvis anmodningen lykkes.

    - Dette er en implementering af en GET-anmodning for at hente alle schedules.
    - IActionResult bruges til at returnere forskellige typer svar afhængigt af resultatet af anmodningen (f.eks. Ok, NotFound).
*/
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var scheduleList = await _scheduleRepository.GetAll();
            return Ok(scheduleList);
        }



        /*
    HTTP GET-metode i ScheduleController til at hente en specifik schedule:

    - [HttpGet("{id}")]
      - Angiver, at dette er en HTTP GET-metode, der forventer en parameter (id) som en del af URL'en.

    - public async Task<IActionResult> GetById(int id)
      - Asynkron metode, der håndterer HTTP GET-anmodninger til at hente en specifik schedule baseret på det angivne id.

    - var schedule = await _scheduleRepository.GetById(id);
      - Kalder GetById-metoden på IScheduleRepository for at hente en schedule med det angivne id fra databasen.

    - if (schedule == null)
        return NotFound();
      - Returnerer et HTTP 404 NotFound-svar, hvis der ikke findes en schedule med det angivne id.

    - return Ok(schedule);
      - Returnerer et HTTP 200 OK-svar med schedule som responskrop, hvis anmodningen lykkes.

    - Dette er en implementering af en GET-anmodning for at hente en specifik schedule.
    - IActionResult bruges til at returnere forskellige typer svar afhængigt af resultatet af anmodningen (f.eks. Ok, NotFound).
*/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _scheduleRepository.GetById(id);

            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(schedule);
        }


        /*
    HTTP POST-metode i ScheduleController til at oprette en ny schedule:

    - [HttpPost]
      - Angiver, at dette er en HTTP POST-metode, der bruges til at oprette ny data.

    - public async Task<IActionResult> Create([FromBody] ScheduleCreateDto scheduleDto)
      - Asynkron metode, der håndterer HTTP POST-anmodninger til at oprette en ny schedule.

    - if (!ModelState.IsValid)
        return BadRequest(ModelState);
      - Validerer ModelState for at sikre, at de modtagne data er gyldige. Returnerer et HTTP 400 BadRequest-svar med eventuelle valideringsfejl, hvis ikke.

    - var scheduleId = await _scheduleRepository.Create(new Schedule
        {
            // Mapping af ScheduleCreateDto til Schedule-entiteten
            subject_id = scheduleDto.subject_id,
            day_of_week = scheduleDto.day_of_week,
            subject_name = scheduleDto.subject_name,
            start_time = scheduleDto.start_time,
            end_time = scheduleDto.end_time,
            class_id = scheduleDto.class_id
        });
      - Kalder Create-metoden på IScheduleRepository for at oprette en ny schedule ved hjælp af dataene fra ScheduleCreateDto.

    - return CreatedAtAction(nameof(GetById), new { id = scheduleId }, null);
      - Returnerer et HTTP 201 Created-svar med en URL til den nyoprettede schedule som responshoved, hvis oprettelsen lykkes.

    - ScheduleCreateDto bruges til at modtage data fra anmodningen.
    - ScheduleRepository håndterer oprettelse af schedules i databasen.
    - IActionResult bruges til at returnere forskellige typer svar afhængigt af resultatet af anmodningen (f.eks. CreatedAtAction, BadRequest).
*/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ScheduleCreateDto scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scheduleId = await _scheduleRepository.Create(new Schedule
            {
                subject_id = scheduleDto.subject_id,
                day_of_week = scheduleDto.day_of_week,
                subject_name = scheduleDto.subject_name,
                start_time = scheduleDto.start_time,
                end_time = scheduleDto.end_time,
                class_id = scheduleDto.class_id
            });

            return CreatedAtAction(nameof(GetById), new { id = scheduleId }, null);
        }




        /*
    HTTP PUT-metode i ScheduleController til at opdatere en eksisterende schedule:

    - [HttpPut("{id}")]
      - Angiver, at dette er en HTTP PUT-metode, der bruges til at opdatere eksisterende data med et specifikt id.

    - public async Task<IActionResult> Update(int id, [FromBody] ScheduleCreateDto scheduleDto)
      - Asynkron metode, der håndterer HTTP PUT-anmodninger til at opdatere en eksisterende schedule med det angivne id.

    - if (!ModelState.IsValid)
        return BadRequest(ModelState);
      - Validerer ModelState for at sikre, at de modtagne data er gyldige. Returnerer et HTTP 400 BadRequest-svar med eventuelle valideringsfejl, hvis ikke.

    - try
      {
          await _scheduleRepository.Update(id, scheduleDto);
          return NoContent();
      }
      catch (ArgumentException ex)
      {
          return NotFound(ex.Message);
      }
    - Kalder Update-metoden på IScheduleRepository for at opdatere schedule'en med det angivne id ved hjælp af dataene fra ScheduleCreateDto.
    - Returnerer et HTTP 204 NoContent-svar, hvis opdateringen lykkes.
    - Returnerer et HTTP 404 NotFound-svar med en fejlmeddelelse, hvis schedule'en med det angivne id ikke blev fundet.

    - ScheduleCreateDto bruges til at modtage opdateringsdata fra anmodningen.
    - ScheduleRepository håndterer opdatering af schedules i databasen.
    - IActionResult bruges til at returnere forskellige typer svar afhængigt af resultatet af anmodningen (f.eks. NoContent, NotFound).
*/
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduleCreateDto scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _scheduleRepository.Update(id, scheduleDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }


        /*
    HTTP DELETE-metode i ScheduleController til at slette en eksisterende schedule:

    - [HttpDelete("{id}")]
      - Angiver, at dette er en HTTP DELETE-metode, der bruges til at slette data med et specifikt id.

    - public async Task<IActionResult> Delete(int id)
      - Asynkron metode, der håndterer HTTP DELETE-anmodninger til at slette en eksisterende schedule med det angivne id.

    - try
      {
          await _scheduleRepository.Delete(id);
          return NoContent();
      }
      catch (ArgumentException ex)
      {
          return NotFound(ex.Message);
      }
    - Kalder Delete-metoden på IScheduleRepository for at slette schedule'en med det angivne id.
    - Returnerer et HTTP 204 NoContent-svar, hvis sletningen lykkes.
    - Returnerer et HTTP 404 NotFound-svar med en fejlmeddelelse, hvis schedule'en med det angivne id ikke blev fundet.

    - ScheduleRepository håndterer sletning af schedules i databasen.
    - IActionResult bruges til at returnere forskellige typer svar afhængigt af resultatet af anmodningen (f.eks. NoContent, NotFound).
*/
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _scheduleRepository.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}