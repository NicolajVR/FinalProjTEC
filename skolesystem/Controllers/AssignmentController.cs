using System;
using Microsoft.AspNetCore.Mvc;
using skolesystem.Authorization;
using skolesystem.DTOs.Assignment.Request;
using skolesystem.DTOs.Assignment.Response;
using skolesystem.Service.AssignmentService;

namespace skolesystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
	{
        private readonly IAssignmentService _AssignmentService;

        public AssignmentController(IAssignmentService AssignmentsService)
        {
            _AssignmentService = AssignmentsService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Kald service-metode for at hente alle assignments
                List<AssignmentResponse> AssignmentList = await _AssignmentService.GetAll();
                // Håndter tilfælde, hvor der ikke er nogen data
                if (AssignmentList == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }
                // Håndter tilfælde, hvor der er en tom liste
                if (AssignmentList.Count == 0)
                {
                    return NoContent();
                }
                // Returner HTTP-statuskode 200 OK med assignments som svar
                return Ok(AssignmentList);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(2)]// Autoriser kun brugere med roller 2(lærer)
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                // Kald service-metode for at hente alle assignments
                AssignmentResponse Assignments = await _AssignmentService.GetById(id);
                // Håndter tilfælde, hvor der ikke er nogen data
                if (Assignments == null)
                {
                    return NotFound();
                }
                // Returner HTTP-statuskode 200 OK med assignments som svar
                return Ok(Assignments);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(2)]// Autoriser kun brugere med roller 2(lærer)
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] NewAssignment newAssignment)
        {
            try
            {
                // Kald service-metode for at oprette en ny assignment
                AssignmentResponse Assignments = await _AssignmentService.Create(newAssignment);

                if (Assignments == null)
                {
                    return Problem("Product was not created, something went wrong");
                }

                return Ok(Assignments);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(2)]// Autoriser kun brugere med roller 2(lærer)
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int id/*modtager id fra frontend*/,
            [FromBody] UpdateAssignment updateAssignment/*modtager data*/)
        {
            try
            {
                // Kald service-metode for at opdatere en eksisterende assignment
                AssignmentResponse Assignments = await _AssignmentService.Update(id, updateAssignment);

                if (Assignments == null)
                {
                    return Problem("Product was not updated, something went wrong");
                }

                return Ok(Assignments);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(2)]// Autoriser kun brugere med roller 2(lærer)
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                // Kald service-metode for at slette en assignment baseret på id
                bool result = await _AssignmentService.Delete(id);

                if (!result)
                {
                    return Problem("Assignment was not deleted, something went wrong");
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

