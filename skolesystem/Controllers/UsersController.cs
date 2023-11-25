using Microsoft.AspNetCore.Mvc;
using skolesystem.Authorization;
using skolesystem.Data;
using skolesystem.DTOs;
using skolesystem.Service;
using Users = skolesystem.Models.Users;


namespace skolesystem.Controllers
{
    /*
    UserController er en API-controller, der håndterer HTTP-anmodninger relateret til brugere.

    - [Route("api/[controller]")]
      - Angiver, at API-ruten skal være "api/[controller]", hvor "[controller]" erstattes med controllerens navn.

    - [ApiController]
      - Angiver, at denne klasse fungerer som en API-controller og aktiverer nogle standard API-relaterede konventioner.

    - public class UserController : ControllerBase
      - Klasse, der nedarver fra ControllerBase og fungerer som en controller for API-relaterede handlinger for brugere.

    - private readonly UsersDbContext _context;
      - Privat felt, der indeholder en instans af UsersDbContext, bruges til at arbejde med databasen.

    - private readonly IUsersService _usersService;
      - Privat felt, der indeholder en instans af IUsersService, bruges til at udføre operationer på brugerdata.

    - private readonly IJwtUtils _jwtUtils;
      - Privat felt, der indeholder en instans af IJwtUtils, sandsynligvis brugt til at håndtere JWT (JSON Web Token) -funktionalitet.

    - public UserController(UsersDbContext context, IUsersService usersService, IJwtUtils jwtUtils)
      - Konstruktør, der modtager en instans af UsersDbContext, IUsersService og IJwtUtils som parametre.

    - _context = context;
      - Initialiserer det private felt _context med den modtagne instans af UsersDbContext.

    - _usersService = usersService;
      - Initialiserer det private felt _usersService med den modtagne instans af IUsersService.

    - _jwtUtils = jwtUtils;
      - Initialiserer det private felt _jwtUtils med den modtagne instans af IJwtUtils.
*/
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UsersDbContext _context;
        private readonly IUsersService _usersService;
        private readonly IJwtUtils _jwtUtils;

        public UserController(UsersDbContext context, IUsersService usersService, IJwtUtils jwtUtils)
        {
            _context = context;
            _usersService = usersService;

            _jwtUtils = jwtUtils;
        }



        /*
    [AllowAnonymous]
    - Angiver, at denne handling kan udføres af enhver, selvom de ikke er godkendt.

    [HttpPost("authenticate")]
    - Angiver, at denne handling håndterer HTTP POST-anmodninger på ruten "authenticate" under "api/[controller]".

    public async Task<IActionResult> Authenticate(LoginRequest login)
    - En asynkron handling, der tager en LoginRequest som input og returnerer en IActionResult.

    try
    {
        var response = await _usersService.Authenticate(login);
        - Kalder Authenticate-metoden på _usersService og venter på resultatet.

        if (response == null)
        {
            return Unauthorized();
        }
        - Hvis Authenticate-metoden returnerer null, returneres et HTTP 401 Unauthorized-svar.

        return Ok(response);
        - Hvis Authenticate-metoden returnerer en respons, sendes en HTTP 200 OK med responsen.

    }
    catch (Exception ex)
    {
        return StatusCode(500, "An error occurred while processing the request.");
    }
    - Hvis der opstår en exception under udførelsen af handlingen, returneres en HTTP 500 Internal Server Error med en passende fejlmeddelelse.
*/
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginRequest login)
        {
            try
            {
                var response = await _usersService.Authenticate(login);

                if (response == null)
                {
                    return Unauthorized();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }



        /*
    [Authorize(1)]
    - Kræver, at brugeren er godkendt og har mindst den specificerede rolle (rolle med ID 1) for at få adgang til denne handling.

    [HttpGet]
    - Angiver, at denne handling håndterer HTTP GET-anmodninger.

    public async Task<IEnumerable<UserReadDto>> GetUsers()
    - En asynkron handling, der returnerer en samling af UserReadDto-objekter.

    var users = await _usersService.GetAllUsers();
    - Kalder GetAllUsers-metoden på _usersService og venter på resultatet.

    var userDtos = new List<UserReadDto>();
    - Opretter en liste af UserReadDto-objekter.

    foreach (var user in users)
    {
        userDtos.Add(new UserReadDto
        {
            user_id = user.user_id,
            surname = user.surname,
            email = user.email,
            password_hash = user.password_hash,
            is_deleted = user.is_deleted,
            role_id = user.role_id
        });
    }
    - Itererer gennem brugerne og mapper dem til UserReadDto-objekter, som tilføjes til listen.

    return userDtos;
    - Returnerer listen af UserReadDto-objekter som svar på GET-anmodningen.
*/
        [Authorize(1)]
        [HttpGet]
        public async Task<IEnumerable<UserReadDto>> GetUsers()
        {
            var users = await _usersService.GetAllUsers();
            var userDtos = new List<UserReadDto>();
            foreach (var user in users)
            {
                userDtos.Add(new UserReadDto
                {
                    user_id = user.user_id,
                    surname = user.surname,
                    email = user.email,
                    is_deleted = user.is_deleted,
                    role_id = user.role_id
                });
            }
            return userDtos;
        }


        /*
    [HttpGet("{id}")]
    - Angiver, at denne handling håndterer HTTP GET-anmodninger med en yderligere parameter 'id'.

    [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
    - Specificerer det forventede responstypen og HTTP-statuskode, når anmodningen er vellykket (200 OK).

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    - Specificerer HTTP-statuskoden, når ressourcen ikke findes (404 Not Found).

    public async Task<IActionResult> GetUserById(int id)
    - En asynkron handling, der tager en 'id'-parameter og returnerer en IActionResult.

    var user = await _usersService.GetUserById(id);
    - Kalder GetUserById-metoden på _usersService og venter på resultatet.

    if (user == null)
    {
        return NotFound();
    }
    - Tjekker om brugeren blev fundet. Hvis ikke, returneres en HTTP 404 Not Found-status.

    var userDto = new UserReadDto
    {
        user_id = user.user_id,
        surname = user.surname,
        email = user.email,
        password_hash = user.password_hash,
        is_deleted = user.is_deleted,
        role_id = user.role_id
    };
    - Opretter et UserReadDto-objekt ved at mappe egenskaberne fra det fundne brugerobjekt.

    return Ok(userDto);
    - Returnerer HTTP 200 OK sammen med det mappede UserReadDto-objekt som svar på GET-anmodningen.
*/
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _usersService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserReadDto
            {
                user_id = user.user_id,
                surname = user.surname,
                email = user.email,
                is_deleted = user.is_deleted,
                role_id = user.role_id
            };

            return Ok(userDto);
        }



        /*
    [Authorize(1)]
    - Kræver, at brugeren har en specifik rolle (rolle_id = 1) for at få adgang til denne handling.

    [HttpPost]
    - Angiver, at denne handling håndterer HTTP POST-anmodninger.

    [ProducesResponseType(StatusCodes.Status201Created)]
    - Specificerer HTTP-statuskoden for oprettelse af en ressource (201 Created).

    [ProducesResponseType(StatusCodes.Status200OK)]
    - Specificerer HTTP-statuskoden for en vellykket anmodning (200 OK).

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    - Specificerer HTTP-statuskoden for en ugyldig anmodning (400 Bad Request).

    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    - Specificerer HTTP-statuskoden for en intern serverfejl (500 Internal Server Error).

    public async Task<IActionResult> CreateUser(UserCreateDto userDto)
    - En asynkron handling, der tager en UserCreateDto som input og returnerer en IActionResult.

    try
    {
        ...
    }
    catch (Exception ex)
    {
        return StatusCode(500, "An error occurred while processing the request.");
    }
    - Forsøger at udføre koden inden for try-blokken. Hvis der opstår en undtagelse, returneres en HTTP 500 Internal Server Error-status.

    string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
    - Genererer en "salt" ved hjælp af BCrypt-biblioteket til at styrke krypteringen af adgangskoden.

    var user = new Users
    {
        surname = userDto.surname,
        email = userDto.email,
        password_hash = userDto.password_hash,
        role_id = userDto.role_id
    };
    - Opretter et brugerobjekt baseret på oplysninger fra userDto.

    userDto.password_hash = BCrypt.Net.BCrypt.HashPassword(user.password_hash);
    - Hasher adgangskoden ved hjælp af BCrypt-biblioteket.

    await _usersService.AddUser(userDto);
    - Kalder AddUser-metoden på _usersService og venter på resultatet.

    return CreatedAtAction(nameof(GetUserById), new { id = user.user_id }, userDto);
    - Returnerer HTTP 201 Created sammen med oprettet brugeroplysninger som svar på POST-anmodningen.
*/
        [Authorize(1)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser(UserCreateDto userDto)
        {
            try
            {

                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                var user = new Users
                {
                    surname = userDto.surname,
                    email = userDto.email,
                    password_hash = userDto.password_hash,
                    role_id = userDto.role_id

                };
                userDto.password_hash = BCrypt.Net.BCrypt.HashPassword(user.password_hash);

                await _usersService.AddUser(userDto);

                return CreatedAtAction(nameof(GetUserById), new { id = user.user_id }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }


        /*
    [HttpPut("{id}")]
    - Angiver, at denne handling håndterer HTTP PUT-anmodninger med en id-parameter.

    public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userDto)
    - En asynkron handling, der tager en id og en UserUpdateDto som input og returnerer en IActionResult.

    await _usersService.UpdateUser(id, userDto);
    - Kalder UpdateUser-metoden på _usersService for at opdatere brugeroplysningerne baseret på id og userDto.

    return Ok();
    - Returnerer HTTP 200 OK som svar på en vellykket opdatering af brugeroplysninger.
*/
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userDto)
        {
            try
            {

                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                var user = new Users
                {
                    surname = userDto.surname,
                    email = userDto.email,
                    password_hash = userDto.password_hash,
                    role_id = userDto.role_id

                };
                userDto.password_hash = BCrypt.Net.BCrypt.HashPassword(user.password_hash);

                await _usersService.UpdateUser(id, userDto);

                return CreatedAtAction(nameof(GetUserById), new { id = user.user_id }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }



        /*
    [HttpDelete("{id}")]
    - Angiver, at denne handling håndterer HTTP DELETE-anmodninger med en id-parameter.

    public async Task<IActionResult> Delete(int id)
    - En asynkron handling, der tager en id som input og returnerer en IActionResult.

    var userToDelete = await _usersService.GetUserById(id);
    - Henter brugeroplysninger baseret på den angivne id ved at kalde GetUserById-metoden på _usersService.

    if (userToDelete == null)
    {
        return NotFound();
    }
    - Returnerer HTTP 404 Not Found, hvis brugeren ikke findes.

    await _usersService.SoftDeleteUser(id);
    - Kalder SoftDeleteUser-metoden på _usersService for at markere brugeren som slettet.

    return NoContent();
    - Returnerer HTTP 204 No Content som svar på en vellykket sletning af brugeren.
*/
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userToDelete = await _usersService.GetUserById(id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            await _usersService.SoftDeleteUser(id);

            return NoContent();
        }


        /*
    public static UserReadDto MapUserTouserResponse(Users user)
    - En statisk metode, der mapper en Users-entitet til en UserReadDto.

    return new UserReadDto
    {
        user_id = user.user_id,
        surname = user.surname,
        email = user.email,
        role_id = user.role_id
    };
    - Opretter en ny instans af UserReadDto og udfylder egenskaberne med værdier fra Users-entiteten.
*/
        public static UserReadDto MapUserTouserResponse(Users user)
        {
            return new UserReadDto
            {
                user_id = user.user_id,
                surname = user.surname,
                email = user.email,
                role_id = user.role_id
            };
        }

    }
}