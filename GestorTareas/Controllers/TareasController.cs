using FluentValidation;
using GestorTareas.DTOs.Tareas;
using GestorTareasAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorTareasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TareasService service;

        public TareasController(TareasService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult GetTareas()
        {
            var tareas = service.GetAll();

            if (tareas == null || !tareas.Any())
            {
                return NotFound();
            }

            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public IActionResult GetTarea(int id)
        {
            var tarea = service.GetById(id);

            if (tarea == null)
            {
                return NotFound();
            }

            return Ok(tarea);
        }

        [HttpPost]
        public IActionResult CrearTarea(CrearTareaDTO dto)
        {
            try
            {
                service.Insert(dto);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(
                    ex.Errors.Select(x => x.ErrorMessage)
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditarTarea(EditarTareaDTO dto)
        {
            try
            {
                service.Update(dto);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(
                    ex.Errors.Select(x => x.ErrorMessage)
                );
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarTarea(int id)
        {
            try
            {
                service.Delete(id);

                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
