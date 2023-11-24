using System;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;
using skolesystem.Authorization;
using skolesystem.DTOs.Enrollment.Request;
using skolesystem.DTOs.Enrollment.Response;
using skolesystem.Service.EnrollmentService;

namespace skolesystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
	{
        private readonly IEnrollmentService _EnrollmentService;

        public EnrollmentController(IEnrollmentService EnrollmentService)
        {
            _EnrollmentService = EnrollmentService;
        }

        [Authorize(1,2,3)]// Autoriser kun brugere med roller 1(admin) 2(lærer) eller 3(elev)
        [HttpGet("ByUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromRoute] int id)
        {
            try
            {
                // Kald service-metode for at hente alle enrollments
                List<EnrollmentResponse> Enrollments = await _EnrollmentService.GetAllEnrollmentsByUser(id);
                // Håndter tilfælde, hvor der ikke er nogen data
                if (Enrollments == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }
                // Håndter tilfælde, hvor der er en tom liste
                if (Enrollments.Count == 0)
                {
                    return NoContent();
                }
                // Returner HTTP-statuskode 200 OK med enrollments som svar
                return Ok(Enrollments);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [Authorize(1,2,3)]// Autoriser kun brugere med roller 1(admin) 2(lærer) eller 3(elev)
        [HttpGet("ByClass/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllEnrollmentsByClass([FromRoute] int id)
        {
            try
            {
                List<EnrollmentResponse> Enrollments = await _EnrollmentService.GetAllEnrollmentsByClass(id);

                if (Enrollments == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }

                if (Enrollments.Count == 0)
                {
                    return NoContent();
                }

                return Ok(Enrollments);

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
                // Kald service-metode for at hente en enrollment
                EnrollmentResponse Enrollments = await _EnrollmentService.GetById(id);
                // Håndter tilfælde, hvor enrollment ikke er fundet
                if (Enrollments == null)
                {
                    return NotFound();
                }

                return Ok(Enrollments);
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
        public async Task<IActionResult> Create([FromBody] NewEnrollment newEnrollment)
        {
            try
            {
                // Kald service-metode for at oprette en ny enrollment
                EnrollmentResponse Enrollments = await _EnrollmentService.Create(newEnrollment);

                if (Enrollments == null)
                {
                    return Problem("Enrollment was not created, something went wrong");
                }

                return Ok(Enrollments);
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
        public async Task<IActionResult> Update([FromRoute] int id/*modtager id fra frontend*/
            , [FromBody] UpdateEnrollment updateEnrollment/*modtager data*/)
        {
            try
            {
                // Kald service-metode for at opdatere en eksisterende enrollment
                EnrollmentResponse Enrollments = await _EnrollmentService.Update(id, updateEnrollment);

                if (Enrollments == null)
                {
                    return Problem("Enrollment was not updated, something went wrong");
                }

                return Ok(Enrollments);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [Authorize(1)]// Autoriser kun brugere med roller 1(admin)
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                // Kald service-metode for at slette en enrollment baseret på id
                bool result = await _EnrollmentService.Delete(id);

                if (!result)
                {
                    return Problem("Enrollment was not deleted, something went wrong");
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