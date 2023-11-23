using System;
using Microsoft.AspNetCore.Mvc;
using skolesystem.DTOs.Subject.Request;
using skolesystem.DTOs.Subject.Response;
using skolesystem.Service.SubjectService;

namespace skolesystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
	{
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<SubjectResponse> SubjectResponses =
                    await _subjectService.GetAll();

                if (SubjectResponses == null)
                {
                    return Problem("Nothing...");
                }
                if (SubjectResponses.Count == 0)
                {
                    return NoContent();
                }
                return Ok(SubjectResponses);
            }
            catch (Exception exp)
            {
                return Problem(exp.Message);
            }
        }
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            try
            {
                SubjectResponse SubjectResponse =
                    await _subjectService.GetById(Id);

                if (SubjectResponse == null)
                {
                    return Problem("Nothing...");
                }
                return Ok(SubjectResponse);
            }
            catch (Exception exp)
            {
                return Problem(exp.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] NewSubject newSubject)
        {
            try
            {
                SubjectResponse SubjectResponse =
                    await _subjectService.Create(newSubject);

                if (SubjectResponse == null)
                {
                    return Problem("Nothing...");
                }
                return Ok(SubjectResponse);
            }
            catch (Exception exp)
            {

                return Problem(exp.Message);
            }
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int Id,
        [FromBody] UpdateSubject updateSubject)
        {
            try
            {
                SubjectResponse SubjectResponse =
                    await _subjectService.Update(Id, updateSubject);

                if (SubjectResponse == null)
                {
                    return Problem("Nothing...");
                }

                return Ok(SubjectResponse);
            }
            catch (Exception exp)
            {

                return Problem(exp.Message);
            }
        }


    }
}

