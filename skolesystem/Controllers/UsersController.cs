using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using skolesystem.Data;
using skolesystem.DTOs; 
using skolesystem.Models;
using skolesystem.Service;
using skolesystem.Authorization;
using skolesystem.Migrations.UsersDb;
using Users = skolesystem.Models.Users;
using BCrypt.Net;


namespace skolesystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UsersDbContext _context;
        private readonly IUsersService _usersService;
        private readonly IJwtUtils _jwtUtils;

        public UserController(UsersDbContext context,IUsersService usersService, IJwtUtils jwtUtils)
        {
            _context = context;
            _usersService = usersService;

            _jwtUtils = jwtUtils;
        }


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
                // Log the exception or handle it as needed
                return StatusCode(500, "An error occurred while processing the request.");
            }


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userDto)
        {

            await _usersService.UpdateUser(id, userDto);
            return Ok();
        }

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

