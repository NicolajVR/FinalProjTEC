using AutoMapper;
using skolesystem.Authorization;
using skolesystem.DTOs;
using skolesystem.Models;
using skolesystem.Repository;

namespace skolesystem.Service
{
    //Service for business logic
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



        public async Task<UserReadDto> GetUserById(int id)
        {
            var user = await _usersRepository.GetById(id);
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsers()
        {
            var users = await _usersRepository.GetAll();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<IEnumerable<UserReadDto>> GetDeletedUsers()
        {
            var deletedUsers = await _usersRepository.GetDeletedUsers();
            return _mapper.Map<IEnumerable<UserReadDto>>(deletedUsers);
        }

        public async Task AddUser(UserCreateDto user)
        {


            var userEntity = _mapper.Map<Users>(user);
            await _usersRepository.AddUser(userEntity);
        }

        public async Task UpdateUser(int id, UserUpdateDto userDto)
        {
            var existingUser = await _usersRepository.GetById(id);

            if (existingUser == null)
            {
                return;
            }
            userDto.password_hash = BCrypt.Net.BCrypt.HashPassword(userDto.password_hash);


            // Update the properties of existingUser based on userDto
            //existingUser.surname = userDto.surname;
            //existingUser.email = userDto.email;
            _mapper.Map(userDto, existingUser);


            // Save the changes
            await _usersRepository.UpdateUser(existingUser);
        }

        public async Task SoftDeleteUser(int id)
        {
            await _usersRepository.SoftDeleteUser(id);
        }
    }

}
