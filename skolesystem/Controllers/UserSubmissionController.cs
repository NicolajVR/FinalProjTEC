using System;
using Microsoft.AspNetCore.Mvc;
using skolesystem.Authorization;
using skolesystem.DTOs.UserSubmission.Request;
using skolesystem.DTOs.UserSubmission.Response;
using skolesystem.Service.UserSubmissionService;

namespace skolesystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSubmissionController : ControllerBase
	{
        private readonly IUserSubmissionService _UserSubmissionService;

        public UserSubmissionController(IUserSubmissionService UserSubmissionService)
        {
            _UserSubmissionService = UserSubmissionService;
        }

        [Authorize(2,3)]// Autoriser kun brugere med roller 2(lærer) eller 3(elev)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Kald service-metode for at hente alle UserSubmissions
                List<UserSubmissionResponse> UserSubmissions = await _UserSubmissionService.GetAll();
                // Håndter tilfælde, hvor der ikke er nogen data
                if (UserSubmissions == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }
                // Håndter tilfælde, hvor der er en tom liste
                if (UserSubmissions.Count == 0)
                {
                    return NoContent();
                }
                // Returner HTTP-statuskode 200 OK med UserSubmissions som svar
                return Ok(UserSubmissions);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                // Kald service-metode for at hente en UserSubmission
                UserSubmissionResponse UserSubmissions = await _UserSubmissionService.GetById(id);
                // Håndter tilfælde, hvor UserSubmission ikke er fundet
                if (UserSubmissions == null)
                {
                    return NotFound();
                }

                return Ok(UserSubmissions);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] NewUserSubmission newUserSubmission)
        {
            try
            {
                // Kald service-metode for at oprette en ny UserSubmission
                UserSubmissionResponse UserSubmissions = await _UserSubmissionService.Create(newUserSubmission);

                if (UserSubmissions == null)
                {
                    return Problem("UserSubmission was not created, something went wrong");
                }

                return Ok(UserSubmissions);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int id/*modtager id fra frontend*/,
            [FromBody] UpdateUserSubmission updateUserSubmission/*modtager data*/)
        {
            try
            {
                // Kald service-metode for at opdatere en eksisterende UserSubmission
                UserSubmissionResponse UserSubmissions = await _UserSubmissionService.Update(id, updateUserSubmission);

                if (UserSubmissions == null)
                {
                    return Problem("UserSubmission was not updated, something went wrong");
                }

                return Ok(UserSubmissions);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id/*modtager id*/)
        {
            try
            {
                // Kald service-metode for at slette en brugerindsendelse baseret på id
                bool result = await _UserSubmissionService.Delete(id);

                if (!result)
                {
                    return Problem("UserSubmission was not deleted, something went wrong");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

