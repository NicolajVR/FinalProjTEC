using AutoMapper;
using skolesystem;
using skolesystem.DTOs;
using skolesystem.Models;

/*
    public interface IUser_informationService
    - Definerer kontraktmetoder for brugeren af User_informationService.

    Task<User_informationReadDto> GetUser_informationById(int id);
    - Henter brugerinformation baseret på det angivne ID.

    Task<IEnumerable<User_informationReadDto>> GetAllUser_informations();
    - Henter alle brugerinformationer.

    Task<IEnumerable<User_informationReadDto>> GetDeletedUser_informations();
    - Henter alle slettede brugerinformationer.

    Task<User_informationReadDto> AddUser_information(User_informationCreateDto user_informationDto);
    - Opretter ny brugerinformation ved hjælp af oplysningerne fra User_informationCreateDto.

    Task UpdateUser_information(int id, User_informationUpdateDto user_informationDto);
    - Opdaterer eksisterende brugerinformation baseret på det angivne ID og oplysningerne fra User_informationUpdateDto.

    Task SoftDeleteUser_information(int id);
    - Markerer brugerinformation som slettet uden at fjerne den permanent, baseret på det angivne ID.
*/
public interface IUser_informationService
{
    Task<User_informationReadDto> GetUser_informationById(int id);
    Task<IEnumerable<User_informationReadDto>> GetAllUser_informations();
    Task<IEnumerable<User_informationReadDto>> GetDeletedUser_informations();
    Task<User_informationReadDto> AddUser_information(User_informationCreateDto user_informationDto);
    Task UpdateUser_information(int id, User_informationUpdateDto user_informationDto);
    Task SoftDeleteUser_information(int id);
}



/*
    public class User_informationService : IUser_informationService
    - Implementerer IUser_informationService og håndterer forretningslogik for brugerinformation.

    private readonly IUser_informationRepository _user_informationRepository;
    - Repository, der giver adgang til databasen for brugerinformation.

    private readonly IMapper _mapper;
    - AutoMapper, der hjælper med at konvertere mellem DTO'er og entiteter.

    public User_informationService(IUser_informationRepository user_informationRepository, IMapper mapper)
    - Konstruktør, der tager en IUser_informationRepository og IMapper som parametre og initialiserer de tilsvarende private felter.
*/
public class User_informationService : IUser_informationService
{
    private readonly IUser_informationRepository _user_informationRepository;
    private readonly IMapper _mapper;

    public User_informationService(IUser_informationRepository user_informationRepository, IMapper mapper)
    {
        _user_informationRepository = user_informationRepository;
        _mapper = mapper;
    }

    /*
    public async Task<User_informationReadDto> GetUser_informationById(int id)
    - Asynkron metode, der returnerer brugerinformation baseret på ID.

    var user_information = await _user_informationRepository.GetById(id);
    - Kalder repositorymetoden GetById for at få brugerinformation fra databasen baseret på det angivne ID.

    if (user_information == null)
    {
        return null; // or throw an exception or handle accordingly
    }
    - Tjekker om brugerinformationen ikke blev fundet i databasen.

    var user_informationDto = _mapper.Map<User_informationReadDto>(user_information);
    - Mapper den hentede brugerinformation til User_informationReadDto ved hjælp af AutoMapper.

    return user_informationDto;
    - Returnerer det mappede User_informationReadDto.
*/
    public async Task<User_informationReadDto> GetUser_informationById(int id)
    {
        var user_information = await _user_informationRepository.GetById(id);

        if (user_information == null)
        {
            return null;
        }
        var user_informationDto = _mapper.Map<User_informationReadDto>(user_information);
        return user_informationDto;
    }


    /*
    public async Task<IEnumerable<User_informationReadDto>> GetAllUser_informations()
    - Asynkron metode, der returnerer en liste over alle brugerinformationer.

    var user_informations = await _user_informationRepository.GetAll();
    - Kalder repositorymetoden GetAll for at hente alle brugerinformationer fra databasen.

    return _mapper.Map<IEnumerable<User_informationReadDto>>(user_informations);
    - Mapper den hentede liste af brugerinformationer til IEnumerable<User_informationReadDto> ved hjælp af AutoMapper og returnerer resultatet.
*/
    public async Task<IEnumerable<User_informationReadDto>> GetAllUser_informations()
    {
        var user_informations = await _user_informationRepository.GetAll();
        return _mapper.Map<IEnumerable<User_informationReadDto>>(user_informations);
    }


    /*
    public async Task<IEnumerable<User_informationReadDto>> GetDeletedUser_informations()
    - Asynkron metode, der returnerer en liste over slettede brugerinformationer.

    var deletedUser_informations = await _user_informationRepository.GetDeletedUser_informations();
    - Kalder repositorymetoden GetDeletedUser_informations for at hente alle slettede brugerinformationer fra databasen.

    return _mapper.Map<IEnumerable<User_informationReadDto>>(deletedUser_informations);
    - Mapper den hentede liste af slettede brugerinformationer til IEnumerable<User_informationReadDto> ved hjælp af AutoMapper og returnerer resultatet.
*/
    public async Task<IEnumerable<User_informationReadDto>> GetDeletedUser_informations()
    {
        var deletedUser_informations = await _user_informationRepository.GetDeletedUser_informations();
        return _mapper.Map<IEnumerable<User_informationReadDto>>(deletedUser_informations);
    }


    /*
    public async Task<User_informationReadDto> AddUser_information(User_informationCreateDto user_informationDto)
    - Asynkron metode, der opretter en ny brugerinformation baseret på de angivne oplysninger.

    var user_information = new User_information
    - Opretter en ny User_information-entitet med attributterne fra User_informationCreateDto.

    await _user_informationRepository.AddUser_information(user_information);
    - Kalder repositorymetoden AddUser_information for at tilføje den nye brugerinformation til databasen.

    var createdUser_informationDto = _mapper.Map<User_informationReadDto>(user_information);
    - Mapper den oprettede brugerinformation til User_informationReadDto ved hjælp af AutoMapper.

    return createdUser_informationDto;
    - Returnerer det oprettede User_informationReadDto-objekt.
*/
    public async Task<User_informationReadDto> AddUser_information(User_informationCreateDto user_informationDto)
    {
        var user_information = new User_information
        {
            user_information_id = user_informationDto.user_information_id,
            name = user_informationDto.name,
            last_name = user_informationDto.last_name,
            phone = user_informationDto.phone,
            date_of_birth = user_informationDto.date_of_birth,
            address = user_informationDto.address,
            is_deleted = user_informationDto.is_deleted,
            gender_id = user_informationDto.gender_id,
            user_id = user_informationDto.user_id,
        };

        await _user_informationRepository.AddUser_information(user_information);

        var createdUser_informationDto = _mapper.Map<User_informationReadDto>(user_information);

        return createdUser_informationDto;
    }


    /*
    public async Task UpdateUser_information(int id, User_informationUpdateDto user_informationDto)
    - Asynkron metode, der opdaterer brugerinformationen med den angivne id med de nye oplysninger.

    var existingUser_information = await _user_informationRepository.GetById(id);
    - Henter den eksisterende brugerinformation fra databasen baseret på den angivne id.

    if (existingUser_information == null)
    - Tjekker om den eksisterende brugerinformation er null, og kaster en NotFoundException, hvis den ikke findes.

    _mapper.Map(user_informationDto, existingUser_information);
    - Mapper oplysningerne fra User_informationUpdateDto til den eksisterende brugerinformation ved hjælp af AutoMapper.

    await _user_informationRepository.UpdateUser_information(id, existingUser_information);
    - Opdaterer brugerinformationen i databasen ved at kalde repositorymetoden UpdateUser_information.

    // Continue with the update logic
    - Kommentaren antyder, at der kan tilføjes yderligere opdateringslogik her, hvis det er nødvendigt.
*/
    public async Task UpdateUser_information(int id, User_informationUpdateDto user_informationDto)
    {
        var existingUser_information = await _user_informationRepository.GetById(id);

        if (existingUser_information == null)
        {
            throw new NotFoundException("User_information not found");
        }

        _mapper.Map(user_informationDto, existingUser_information);

        await _user_informationRepository.UpdateUser_information(id, existingUser_information);
    }




    /*
    public async Task SoftDeleteUser_information(int id)
    - Asynkron metode, der markerer brugerinformationen som slettet uden at fjerne den permanent.

    var existingUser_information = await _user_informationRepository.GetById(id);
    - Henter den eksisterende brugerinformation fra databasen baseret på den angivne id.

    if (existingUser_information == null)
    - Tjekker om den eksisterende brugerinformation er null, og kaster en NotFoundException, hvis den ikke findes.

    await _user_informationRepository.SoftDeleteUser_information(id);
    - Kalder repositorymetoden SoftDeleteUser_information for at markere brugerinformationen som slettet.
*/
    public async Task SoftDeleteUser_information(int id)
    {
        var existingUser_information = await _user_informationRepository.GetById(id);

        if (existingUser_information == null)
        {
            throw new NotFoundException("User_information not found");
        }

        await _user_informationRepository.SoftDeleteUser_information(id);
    }
}