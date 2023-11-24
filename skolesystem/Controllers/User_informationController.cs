using Microsoft.AspNetCore.Mvc;
using skolesystem.DTOs;

namespace skolesystem.Controllers
{
    /*
    En ASP.NET Core Web API-controller, der håndterer operationer relateret til brugerinformationer.

    - [Route("api/[controller]")]
      - Angiver, at denne controller skal være tilgængelig ved at tilgå "api/[controller]" i URL'en.
    
    - [ApiController]
      - Indikerer, at controlleren skal følge ASP.NET Core's API-controllerkonventioner.

    - public class User_informationController : ControllerBase
      - Deklarerer en controllerklasse, der nedarver fra ControllerBase, hvilket betyder, at det ikke understøtter synlighed af synstilknyttede udsigter og i stedet arbejder med API-forespørgsler og svar.

    - private readonly IUser_informationService _User_informationService;
      - Privat felt til at gemme en reference til IUser_informationService, der håndterer forretningslogik for brugerinformationer.

    - public User_informationController(IUser_informationService user_informationService)
      - En constructor, der tager IUser_informationService som parameter og initialiserer _User_informationService-feltet.

    Bemærk:
    - IUser_informationService bruges til at udføre operationer på brugerinformationer, og dette injiceres ved hjælp af afhængighedsindsprøjtning.
    - ControllerBase er den basale klasse for API-controllers i ASP.NET Core, og det indeholder funktionaliteter, der er fælles for API'er (f.eks. Request, Response).
*/
    [Route("api/[controller]")]
    [ApiController]
    public class User_informationController : ControllerBase
    {
        private readonly IUser_informationService _User_informationService;

        public User_informationController(IUser_informationService user_informationService)
        {
            _User_informationService = user_informationService;

        }



        /*
    HTTP GET-metode i User_informationController til at hente alle brugerinformationer:

    - [HttpGet]
      - Angiver, at dette er en HTTP GET-metode, der bruges til at hente data.

    - public async Task<IEnumerable<User_informationReadDto>> Get()
      - Asynkron metode, der håndterer HTTP GET-anmodninger for at hente alle brugerinformationer.

    - var user_informations = await _User_informationService.GetAllUser_informations();
      - Kalder GetAllUser_informations-metoden på IUser_informationService for at hente alle brugerinformationer fra dataservicen.

    - var user_informationDtos = user_informations.Select(User_information => new User_informationReadDto
      {
          // Mapper data fra User_information til User_informationReadDto
          user_information_id = User_information.user_information_id,
          name = User_information.name,
          last_name = User_information.last_name,
          phone = User_information.phone,
          date_of_birth = User_information.date_of_birth,
          address = User_information.address,
          is_deleted = User_information.is_deleted,
          gender_id = User_information.gender_id,
          user_id = User_information.user_id
      }).ToList();
      - Mapper data fra User_information-objekter til User_informationReadDto-objekter ved hjælp af LINQ Select-metoden.

    - return user_informationDtos;
      - Returnerer en liste af User_informationReadDto-objekter som HTTP-svaret.

    - IUser_informationService bruges til at hente brugerinformationer fra dataservicen.
    - User_informationReadDto bruges til at definere det format, hvori brugerinformationerne returneres.
*/

        [HttpGet]
        public async Task<IEnumerable<User_informationReadDto>> Get()
        {
            var user_informations = await _User_informationService.GetAllUser_informations();
            var user_informationDtos = user_informations.Select(User_information => new User_informationReadDto
            {
                user_information_id = User_information.user_information_id,
                name = User_information.name,
                last_name = User_information.last_name,
                phone = User_information.phone,
                date_of_birth = User_information.date_of_birth,
                address = User_information.address,
                is_deleted = User_information.is_deleted,
                gender_id = User_information.gender_id,
                user_id = User_information.user_id
            }).ToList();

            return user_informationDtos;
        }




        /*
    HTTP GET-metode i User_informationController til at hente brugerinformation baseret på id:

    - [HttpGet("{id}")]
      - Angiver, at dette er en HTTP GET-metode, der bruges til at hente data med et specifikt id.

    - [ProducesResponseType(typeof(User_informationReadDto), StatusCodes.Status200OK)]
      - Specificerer, at et vellykket svar (HTTP 200 OK) skal returnere data i formatet af User_informationReadDto.

    - [ProducesResponseType(StatusCodes.Status404NotFound)]
      - Specificerer, at et svar med status HTTP 404 NotFound skal returnere, hvis brugerinformationen ikke blev fundet.

    - public async Task<IActionResult> GetById(int id)
      - Asynkron metode, der håndterer HTTP GET-anmodninger for at hente brugerinformation baseret på det angivne id.

    - var User_information = await _User_informationService.GetUser_informationById(id);
      - Kalder GetUser_informationById-metoden på IUser_informationService for at hente brugerinformationen med det angivne id.

    - if (User_information == null)
      {
          return NotFound();
      }
      - Returnerer HTTP 404 NotFound-svaret, hvis brugerinformationen ikke blev fundet.

    - var User_informationDto = new User_informationReadDto
      {
          // Mapper data fra User_information til User_informationReadDto
          user_information_id = User_information.user_information_id,
          name = User_information.name,
          last_name = User_information.last_name,
          phone = User_information.phone,
          date_of_birth = User_information.date_of_birth,
          address = User_information.address,
          is_deleted = User_information.is_deleted,
          gender_id = User_information.gender_id,
          user_id = User_information.user_id
      };

    - return Ok(User_informationDto);
      - Returnerer HTTP 200 OK-svaret med brugerinformationen i form af User_informationReadDto.

    - IUser_informationService bruges til at hente brugerinformationen fra dataservicen.
    - User_informationReadDto bruges til at definere det format, hvori brugerinformationen returneres.
*/
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User_informationReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var User_information = await _User_informationService.GetUser_informationById(id);

            if (User_information == null)
            {
                return NotFound();
            }

            var User_informationDto = new User_informationReadDto
            {
                user_information_id = User_information.user_information_id,
                name = User_information.name,
                last_name = User_information.last_name,
                phone = User_information.phone,
                date_of_birth = User_information.date_of_birth,
                address = User_information.address,
                is_deleted = User_information.is_deleted,
                gender_id = User_information.gender_id,
                user_id = User_information.user_id
            };

            return Ok(User_informationDto);
        }





        /*
    HTTP POST-metode i User_informationController til at oprette ny brugerinformation:

    - [HttpPost]
      - Angiver, at dette er en HTTP POST-metode, der bruges til at oprette ny brugerinformation.

    - [ProducesResponseType(StatusCodes.Status201Created)]
      - Specificerer, at et vellykket svar (HTTP 201 Created) skal returnere, når brugerinformationen er blevet oprettet.

    - [ProducesResponseType(StatusCodes.Status409Conflict)]
      - Specificerer, at et svar med status HTTP 409 Conflict skal returneres, hvis der opstår en konflikt (f.eks., hvis brugerinformationen allerede findes).

    - public async Task<IActionResult> Create(User_informationCreateDto user_informationDto)
      - Asynkron metode, der håndterer HTTP POST-anmodninger for at oprette ny brugerinformation.

    - try
      - Blok, der indeholder den logik, der skal forsøges.

    - var createdUser_informationDto = await _User_informationService.AddUser_information(user_informationDto);
      - Kalder AddUser_information-metoden på IUser_informationService for at oprette brugerinformationen og få det oprettede User_informationReadDto.

    - return CreatedAtAction(nameof(GetById), new { id = createdUser_informationDto.user_information_id }, createdUser_informationDto);
      - Returnerer HTTP 201 Created-svaret med URL'en til det oprettede ressource (brugerinformationen) og det oprettede User_informationReadDto.

    - catch (ArgumentException ex)
      {
          return Conflict(ex.Message);
      }
      - Catch-blok, der håndterer undtagelsen, hvis der opstår en konflikt (f.eks., hvis brugerinformationen allerede findes). Returnerer HTTP 409 Conflict-svaret med meddelelsen fra undtagelsen.
*/

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(User_informationCreateDto user_informationDto)
        {
            try
            {
                var createdUser_informationDto = await _User_informationService.AddUser_information(user_informationDto);
                return CreatedAtAction(nameof(GetById), new { id = createdUser_informationDto.user_information_id }, createdUser_informationDto);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
        }



        /*
    HTTP PUT-metode i User_informationController til at opdatere eksisterende brugerinformation:

    - [HttpPut("{id}")]
      - Angiver, at dette er en HTTP PUT-metode, der bruges til at opdatere eksisterende brugerinformation baseret på brugerens ID.

    - public async Task<IActionResult> UpdateUser_information(int id, User_informationUpdateDto User_informationDto)
      - Asynkron metode, der håndterer HTTP PUT-anmodninger for at opdatere eksisterende brugerinformation.

    - try
      - Blok, der indeholder den logik, der skal forsøges.

    - var existingUser_information = await _User_informationService.GetUser_informationById(id);
      - Henter den eksisterende brugerinformation baseret på brugerens ID ved at kalde GetUser_informationById-metoden på IUser_informationService.

    - if (existingUser_information == null)
      {
          throw new NotFoundException("User_information not found");
      }
      - Checker, om den eksisterende brugerinformation er null. Hvis ja, kaster NotFoundException med en fejlmeddelelse.

    - await _User_informationService.UpdateUser_information(id, User_informationDto);
      - Kalder UpdateUser_information-metoden på IUser_informationService for at opdatere brugerinformationen.

    - return NoContent();
      - Returnerer HTTP 204 NoContent-svaret, da der ikke forventes nogen indhold i responsen efter en succesfuld opdatering.

    - catch (NotFoundException)
      {
          return NotFound();
      }
      - Catch-blok, der håndterer NotFoundException, hvis den kastes. Returnerer HTTP 404 NotFound-svaret, da brugerinformationen ikke blev fundet.
*/
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser_information(int id, User_informationUpdateDto User_informationDto)
        {
            try
            {
                var existingUser_information = await _User_informationService.GetUser_informationById(id);

                if (existingUser_information == null)
                {
                    throw new NotFoundException("User_information not found");
                }

                await _User_informationService.UpdateUser_information(id, User_informationDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }





        /*
    HTTP DELETE-metode i User_informationController til at slette brugerinformation:

    - [HttpDelete("{id}")]
      - Angiver, at dette er en HTTP DELETE-metode, der bruges til at slette brugerinformation baseret på brugerens ID.

    - [ProducesResponseType(StatusCodes.Status204NoContent)]
      - Specificerer, at et HTTP 204 NoContent-svar forventes ved succesfuld sletning.

    - [ProducesResponseType(StatusCodes.Status404NotFound)]
      - Specificerer, at et HTTP 404 NotFound-svar forventes, hvis brugerinformationen ikke blev fundet.

    - public async Task<IActionResult> DeleteUser_information(int id)
      - Asynkron metode, der håndterer HTTP DELETE-anmodninger for at slette brugerinformation.

    - try
      - Blok, der indeholder den logik, der skal forsøges.

    - await _User_informationService.SoftDeleteUser_information(id);
      - Kalder SoftDeleteUser_information-metoden på IUser_informationService for at markere brugerinformationen som slettet.

    - return NoContent();
      - Returnerer HTTP 204 NoContent-svaret, da der ikke forventes nogen indhold i responsen efter en succesfuld sletning.

    - catch (NotFoundException)
      {
          return NotFound();
      }
      - Catch-blok, der håndterer NotFoundException, hvis den kastes. Returnerer HTTP 404 NotFound-svaret, da brugerinformationen ikke blev fundet.
*/
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser_information(int id)
        {
            try
            {
                await _User_informationService.SoftDeleteUser_information(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }
    }
}