using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skolesystem.Authorization;
using skolesystem.Data;
using skolesystem.DTOs;
using skolesystem.Models;

namespace skolesystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_informationController : ControllerBase
    {
        private readonly IUser_informationService _User_informationService;

        public User_informationController(IUser_informationService user_informationService)
        {
            _User_informationService = user_informationService;

        }


        [HttpGet]
        public async Task<IEnumerable<User_informationReadDto>> Get()
        {
            var user_informations= await _User_informationService.GetAllUser_informations();
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





        //[Authorize(2)]
        //[HttpGet("students")]
        //public async Task<IEnumerable<User_information>> GetAllStudents()
        //{
        //    return await _Context.User_information.Where(b => b.is_deleted == false && b.user_id == 3).ToListAsync();

        //}


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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> Create(User_informationCreateDto user_informationDto)
        {
            try
            {
                // Your logic to create the User_information entity and get the created User_informationReadDto
                var createdUser_informationDto = await _User_informationService.AddUser_information(user_informationDto);

                // Return the created User_informationReadDto
                return CreatedAtAction(nameof(GetById), new { id = createdUser_informationDto.user_information_id }, createdUser_informationDto);
            }
            catch (ArgumentException ex)
            {
                // User with the specified ID already exists
                return Conflict(ex.Message);
            }
        }

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