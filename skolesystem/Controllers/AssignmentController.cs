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
                List<AssignmentResponse> AssignmentList = await _AssignmentService.GetAll();

                if (AssignmentList == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }

                if (AssignmentList.Count == 0)
                {
                    return NoContent();
                }

                return Ok(AssignmentList);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(2)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                AssignmentResponse Assignments = await _AssignmentService.GetById(id);

                if (Assignments == null)
                {
                    return NotFound();
                }

                return Ok(Assignments);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(2)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] NewAssignment newAssignment)
        {
            try
            {
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

        [Authorize(2)]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAssignment updateAssignment)
        {
            try
            {
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

        [Authorize(2)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
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

