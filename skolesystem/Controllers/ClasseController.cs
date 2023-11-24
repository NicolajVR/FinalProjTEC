using System;
using System.Runtime.Intrinsics.X86;
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
                // Kald service-metode for at hente alle Classs
                List<ClasseResponse> ClasseResponses =
                    await _ClasseService.GetAll();
                // Håndter tilfælde, hvor der ikke er nogen data
                if (ClasseResponses == null)
                {
                    return Problem("Nothing...");
                }
                // Håndter tilfælde, hvor der er en tom liste
                if (ClasseResponses.Count == 0)
                {
                    return NoContent();
                }
                // Returner HTTP-statuskode 200 OK med Classs som svar
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
                // Kald service-metode for at hente en Class
                ClasseResponse ClasseResponse =
                    await _ClasseService.GetById(Id);
                // Håndter tilfælde, hvor Class ikke er fundet
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
                // Kald service-metode for at oprette en ny Class
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
        public async Task<IActionResult> Update([FromRoute] int Id/*modtager id fra frontend*/,
        [FromBody] UpdateClasse updateClasse/*modtager data*/)
        {
            try
            {
                // Kald service-metode for at opdatere en eksisterende Class
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
