using FluentValidation;
using GestorTareas.DTOs.Tareas;
using GestorTareasAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestorTareasAPI.Controllers
{
    [Authorize]
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
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var tareas = service.GetAll(usuarioId);

            if (!tareas.Any())
            {
                return NotFound();
            }

            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public IActionResult GetTarea(int id)
        {
            var usuarioId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var tarea = service.GetById(id, usuarioId);

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
                var usuarioId = int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                service.Insert(dto, usuarioId);

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
            var usuarioId = int.Parse(
       User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var resultado = service.Update(dto, usuarioId);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarTarea(int id)
        {
            var usuarioId = int.Parse(
      User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var resultado = service.Delete(id, usuarioId);

            if (!resultado)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
