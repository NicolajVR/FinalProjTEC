using System;
using Microsoft.AspNetCore.Mvc;
using skolesystem.DTOs.Classe.Request;
using skolesystem.DTOs.Classe.Response;
using skolesystem.Service.ClasseService;

namespace skolesystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClasseController : ControllerBase
    {
        private readonly IClasseService _ClasseService;

        public ClasseController(IClasseService ClasseService)
        {
            _ClasseService = ClasseService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ClasseResponse> ClasseResponses =
                    await _ClasseService.GetAll();

                if (ClasseResponses == null)
                {
                    return Problem("Nothing...");
                }
                if (ClasseResponses.Count == 0)
                {
                    return NoContent();
                }
                return Ok(ClasseResponses);
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
                ClasseResponse ClasseResponse =
                    await _ClasseService.GetById(Id);

                if (ClasseResponse == null)
                {
                    return Problem("Nothing...");
                }
                return Ok(ClasseResponse);
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
        public async Task<IActionResult> Create([FromBody] NewClasse newClasse)
        {
            try
            {
                ClasseResponse ClasseResponse =
                    await _ClasseService.Create(newClasse);

                if (ClasseResponse == null)
                {
                    return Problem("Nothing...");
                }
                return Ok(ClasseResponse);
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
        [FromBody] UpdateClasse updateClasse)
        {
            try
            {
                ClasseResponse ClasseResponse =
                    await _ClasseService.Update(Id, updateClasse);

                if (ClasseResponse == null)
                {
                    return Problem("Nothing...");
                }

                return Ok(ClasseResponse);
            }
            catch (Exception exp)
            {

                return Problem(exp.Message);
            }
        }

    }
}
