using FluentValidation;
using GestorTareasAPI.Models.DTOs;
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

        private int? GetUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return null;

            if (!int.TryParse(claim.Value, out int usuarioId))
                return null;

            return usuarioId;
        }

        [HttpGet]
        public IActionResult GetTareas()
        {
            var usuarioId = GetUsuarioId();

            if (usuarioId == null)
                return Unauthorized();

            var tareas = service.GetAll(usuarioId.Value);

            if (!tareas.Any())
                return NotFound();

            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public IActionResult GetTarea(int id)
        {
            var usuarioId = GetUsuarioId();

            if (usuarioId == null)
                return Unauthorized();

            var tarea = service.GetById(id, usuarioId.Value);

            if (tarea == null)
                return NotFound();

            return Ok(tarea);
        }

        [HttpPost]
        public IActionResult CrearTarea(CrearTareaDTO dto)
        {
            try
            {
                var usuarioId = GetUsuarioId();

                if (usuarioId == null)
                    return Unauthorized();

                service.Insert(dto, usuarioId.Value);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(x => x.ErrorMessage));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditarTarea(EditarTareaDTO dto)
        {
            var usuarioId = GetUsuarioId();

            if (usuarioId == null)
                return Unauthorized();

            var resultado = service.Update(dto, usuarioId.Value);

            if (!resultado)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarTarea(int id)
        {
            var usuarioId = GetUsuarioId();

            if (usuarioId == null)
                return Unauthorized();

            var resultado = service.Delete(id, usuarioId.Value);

            if (!resultado)
                return NotFound();

            return Ok();
        }
    }
}
