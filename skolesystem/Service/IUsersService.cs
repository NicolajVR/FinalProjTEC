using AutoMapper;
using skolesystem.Authorization;
using skolesystem.DTOs;
using skolesystem.Models;
using skolesystem.Repository;

namespace skolesystem.Service
{
    /*
    public interface IUsersService
    - Interface, der definerer kontrakter for brugerrelaterede operationer.

    Task<UserReadDto> GetUserById(int id);
    - Asynkron metode, der henter en bruger baseret på angivet id.

    Task<IEnumerable<UserReadDto>> GetAllUsers();
    - Asynkron metode, der henter alle brugere.

    Task<IEnumerable<UserReadDto>> GetDeletedUsers();
    - Asynkron metode, der henter alle slettede brugere.

    Task AddUser(UserCreateDto user);
    - Asynkron metode, der tilføjer en ny bruger baseret på de givne oplysninger.

    Task UpdateUser(int id, UserUpdateDto user);
    - Asynkron metode, der opdaterer en eksisterende bruger baseret på angivet id og opdaterede oplysninger.

    Task SoftDeleteUser(int id);
    - Asynkron metode, der markerer en bruger som slettet uden at fjerne den permanent.

    Task<LoginResponse> Authenticate(LoginRequest login);
    - Asynkron metode, der håndterer autentificering af brugeren baseret på loginoplysninger og returnerer et LoginResponse-objekt.
*/
    public interface IUsersService
    {
        Task<UserReadDto> GetUserById(int id);
        Task<IEnumerable<UserReadDto>> GetAllUsers();
        Task<IEnumerable<UserReadDto>> GetDeletedUsers();
        Task AddUser(UserCreateDto user);
        Task UpdateUser(int id, UserUpdateDto user);
        Task SoftDeleteUser(int id);
        Task<LoginResponse> Authenticate(LoginRequest login);
    }


    /*
    public class UsersService : IUsersService
    - Implementering af IUsersService, der håndterer brugerrelaterede operationer.

    private readonly IUsersRepository _usersRepository;
    - Privat variabel til repository-objektet, der bruges til at udføre databaserelaterede operationer.

    private readonly IJwtUtils _jwtUtils;
    - Privat variabel til JwtUtils-objektet, der bruges til at generere og håndtere JWT-token.

    private readonly IMapper _mapper;
    - Privat variabel til mapper-objektet, der bruges til at udføre objektmapping mellem forskellige typer.

    public UsersService(IUsersRepository usersRepository, IJwtUtils jwtUtils, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }
    - Konstruktør, der modtager nødvendige afhængigheder (repositories, utils og mapper) via dependency injection.

    Implementering af IUsersService-metoder følger.
*/
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepository, IJwtUtils jwtUtils, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }




        /*
    public async Task<LoginResponse> Authenticate(LoginRequest login)
    - Implementering af Authenticate-metoden, der håndterer brugerautorisation.

    Users user = await _usersRepository.GetBySurname(login.surname);
    - Forsøger at hente brugeren fra repository baseret på efternavn.

    if (user == null)
    {
        return null;
    }
    - Hvis brugeren ikke findes, returneres null.

    if (BCrypt.Net.BCrypt.Verify(login.password_hash, user.password_hash))
    - Hvis det indtastede password matcher det gemte hashed password, fortsæt.

    LoginResponse response = new()
    {
        user_id = user.user_id,
        surname = user.surname,
        email = user.email,
        role_id = user.role_id,
        is_deleted = user.is_deleted,
        Token = _jwtUtils.GenerateJwtToken(user)
    };
    - Opretter et LoginResponse-objekt med brugeroplysninger og et genereret JWT-token.

    return response;
    - Returnerer LoginResponse-objektet.

    return null;
    - Hvis password ikke matcher, returneres null.

    Implementeringen bruger BCrypt.Net til at verificere password. 
    JWT-token genereres ved hjælp af JwtUtils-objektet.
*/
        public async Task<LoginResponse> Authenticate(LoginRequest login)
        {
            Users user = await _usersRepository.GetBySurname(login.surname);
            if (user == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(login.password_hash, user.password_hash))
            {
                LoginResponse response = new()
                {
                    user_id = user.user_id,
                    surname = user.surname,
                    email = user.email,
                    role_id = user.role_id,
                    is_deleted = user.is_deleted,
                    Token = _jwtUtils.GenerateJwtToken(user)
                };
                return response;
            }
            return null;
        }


        /*
    public async Task<UserReadDto> GetUserById(int id)
    - Implementering af GetUserById-metoden, der henter en bruger baseret på ID.

    var user = await _usersRepository.GetById(id);
    - Forsøger at hente brugeren fra repository baseret på ID.

    return _mapper.Map<UserReadDto>(user);
    - Mapper brugerobjektet til UserReadDto og returnerer det.

    GetUserById-metoden bruger AutoMapper (_mapper) til at mappe brugerobjektet til dets læsbare DTO-ækvivalent (UserReadDto).
*/
        public async Task<UserReadDto> GetUserById(int id)
        {
            var user = await _usersRepository.GetById(id);
            return _mapper.Map<UserReadDto>(user);
        }


        /*
    public async Task<IEnumerable<UserReadDto>> GetAllUsers()
    - Implementering af GetAllUsers-metoden, der henter alle brugere.

    var users = await _usersRepository.GetAll();
    - Forsøger at hente alle brugere fra repository.

    return _mapper.Map<IEnumerable<UserReadDto>>(users);
    - Mapper listen af brugerobjekter til en liste af deres læsbare DTO-ækvivalenter (UserReadDto) og returnerer den.

    GetAllUsers-metoden bruger AutoMapper (_mapper) til at mappe brugerobjekterne til deres DTO-ækvivalenter.
*/
        public async Task<IEnumerable<UserReadDto>> GetAllUsers()
        {
            var users = await _usersRepository.GetAll();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }


        /*
    public async Task<IEnumerable<UserReadDto>> GetDeletedUsers()
    - Implementering af GetDeletedUsers-metoden, der henter alle slettede brugere.

    var deletedUsers = await _usersRepository.GetDeletedUsers();
    - Forsøger at hente alle slettede brugere fra repository.

    return _mapper.Map<IEnumerable<UserReadDto>>(deletedUsers);
    - Mapper listen af slettede brugerobjekter til en liste af deres læsbare DTO-ækvivalenter (UserReadDto) og returnerer den.

    GetDeletedUsers-metoden bruger AutoMapper (_mapper) til at mappe slettede brugerobjekter til deres DTO-ækvivalenter.
*/
        public async Task<IEnumerable<UserReadDto>> GetDeletedUsers()
        {
            var deletedUsers = await _usersRepository.GetDeletedUsers();
            return _mapper.Map<IEnumerable<UserReadDto>>(deletedUsers);
        }


        /*
    public async Task AddUser(UserCreateDto user)
    - Implementering af AddUser-metoden, der tilføjer en ny bruger.

    var userEntity = _mapper.Map<Users>(user);
    - Mapper brugeropretelses-DTO (UserCreateDto) til brugerentiteten (Users).

    await _usersRepository.AddUser(userEntity);
    - Tilføjer den mappede brugerentitet til repository ved hjælp af _usersRepository.

    AddUser-metoden bruger AutoMapper (_mapper) til at mappe brugeropretelses-DTO til brugerentiteten og derefter tilføje brugeren til repository.
*/
        public async Task AddUser(UserCreateDto user)
        {
            var userEntity = _mapper.Map<Users>(user);
            await _usersRepository.AddUser(userEntity);
        }


        /*
    public async Task UpdateUser(int id, UserUpdateDto userDto)
    - Implementering af UpdateUser-metoden, der opdaterer eksisterende brugeroplysninger.

    var existingUser = await _usersRepository.GetById(id);
    - Henter den eksisterende bruger fra repository baseret på brugerens ID.

    if (existingUser == null)
    {
        return;
    }
    - Hvis brugeren ikke findes (er null), afsluttes metoden. Ingen opdatering udføres.

    _mapper.Map(userDto, existingUser);
    - Mapper egenskaber fra brugeropdaterings-DTO (UserUpdateDto) til den eksisterende brugerentitet.

    await _usersRepository.UpdateUser(existingUser);
    - Opdaterer den eksisterende bruger i repository ved hjælp af _usersRepository.
    
    UpdateUser-metoden bruger AutoMapper (_mapper) til at overføre opdaterede egenskaber fra brugeropdaterings-DTO til den eksisterende brugerentitet og derefter gemme ændringerne i repository.
*/
        public async Task UpdateUser(int id, UserUpdateDto userDto)
        {
            var existingUser = await _usersRepository.GetById(id);

            if (existingUser == null)
            {
                return;
            }

            _mapper.Map(userDto, existingUser);

            await _usersRepository.UpdateUser(existingUser);
        }


        /*
    public async Task SoftDeleteUser(int id)
    - Implementering af SoftDeleteUser-metoden, der markerer en bruger som slettet uden at fjerne den fra databasen.

    await _usersRepository.SoftDeleteUser(id);
    - Kalder _usersRepository.SoftDeleteUser(id) for at udføre den soft delete af brugeren i repository.

    SoftDeleteUser-metoden hjælper med at implementere soft delete ved at markere en bruger som slettet uden at fjerne den fysisk fra databasen. Dette opnås ved at indstille en attribut, såsom 'is_deleted', i stedet for at fjerne hele posten.
*/
        public async Task SoftDeleteUser(int id)
        {
            await _usersRepository.SoftDeleteUser(id);
        }
    }

}