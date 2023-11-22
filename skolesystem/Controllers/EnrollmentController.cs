using System;
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

        [Authorize(1,2) ]
        [HttpGet("ByUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromRoute] int id)
        {
            try
            {
                List<EnrollmentResponse> Enrollments = await _EnrollmentService.GetAllEnrollmentsByUser(id);

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
        [Authorize(1,2,3)]
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
                EnrollmentResponse Enrollments = await _EnrollmentService.GetById(id);

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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEnrollment updateEnrollment)
        {
            try
            {
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
        [Authorize(1)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
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