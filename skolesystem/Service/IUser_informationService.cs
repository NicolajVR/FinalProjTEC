using AutoMapper;
using skolesystem;
using skolesystem.DTOs;
using skolesystem.Models;

// Service for business logic
public interface IUser_informationService
{
    Task<User_informationReadDto> GetUser_informationById(int id);
    Task<IEnumerable<User_informationReadDto>> GetAllUser_informations();
    Task<IEnumerable<User_informationReadDto>> GetDeletedUser_informations();
    Task<User_informationReadDto> AddUser_information(User_informationCreateDto user_informationDto);
    Task UpdateUser_information(int id, User_informationUpdateDto user_informationDto);
    Task SoftDeleteUser_information(int id);
}

public class User_informationService : IUser_informationService
{
    private readonly IUser_informationRepository _user_informationRepository;
    private readonly IMapper _mapper;

    public User_informationService(IUser_informationRepository user_informationRepository, IMapper mapper)
    {
        _user_informationRepository = user_informationRepository;
        _mapper = mapper;
    }

    public async Task<User_informationReadDto> GetUser_informationById(int id)
    {
        var user_information = await _user_informationRepository.GetById(id);

        if (user_information == null)
        {
            return null; // or throw an exception or handle accordingly
        }
        var user_informationDto = _mapper.Map<User_informationReadDto>(user_information);
        return user_informationDto;
    }

    public async Task<IEnumerable<User_informationReadDto>> GetAllUser_informations()
    {
        var user_informations = await _user_informationRepository.GetAll();
        return _mapper.Map<IEnumerable<User_informationReadDto>>(user_informations);
    }

    public async Task<IEnumerable<User_informationReadDto>> GetDeletedUser_informations()
    {
        var deletedUser_informations = await _user_informationRepository.GetDeletedUser_informations();
        return _mapper.Map<IEnumerable<User_informationReadDto>>(deletedUser_informations);
    }

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

    public async Task UpdateUser_information(int id, User_informationUpdateDto user_informationDto)
    {
        var existingUser_information = await _user_informationRepository.GetById(id);

        if (existingUser_information == null)
        {
            throw new NotFoundException("User_information not found");
         }

        // Continue with the update logic
        _mapper.Map(user_informationDto, existingUser_information);

        await _user_informationRepository.UpdateUser_information(id, existingUser_information);
    }



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