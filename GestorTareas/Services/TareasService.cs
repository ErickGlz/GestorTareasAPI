using AutoMapper;
using GestorTareasAPI.Models.DTOs;
using GestorTareasAPI.Models.Entities;
using GestorTareasAPI.Repositories;

namespace GestorTareasAPI.Services
{
    public class TareasService
    {
        public TareasService(
            Repository<Tareas> repository,
            IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public Repository<Tareas> Repository { get; }
        public IMapper Mapper { get; }

        public IEnumerable<TareaDTO> GetAll(int usuarioId)
        {
            var tareas = Repository.GetAll()
                .Where(x => x.UsuarioId == usuarioId);

            return Mapper.Map<IEnumerable<TareaDTO>>(tareas);
        }

        public TareaDTO? GetById(int id, int usuarioId)
        {
            var tarea = Repository.Get(id);

            if (tarea == null)
                return null;

            if (tarea.UsuarioId != usuarioId)
                return null;

            return Mapper.Map<TareaDTO>(tarea);
        }

        public void Insert(CrearTareaDTO dto, int usuarioId)
        {
            var tarea = Mapper.Map<Tareas>(dto);

            tarea.UsuarioId = usuarioId;
            tarea.FechaCreacion = DateTime.Now;
            tarea.Completada = false;

            Repository.Insert(tarea);
        }
        public bool Update(EditarTareaDTO dto, int usuarioId)
        {
            var tarea = Repository.Get(dto.Id);

            if (tarea == null)
                return false;

            if (tarea.UsuarioId != usuarioId)
                return false;

            tarea.Titulo = dto.Titulo;
            tarea.Descripcion = dto.Descripcion;
            tarea.FechaLimite = dto.FechaLimite;
            tarea.Prioridad = dto.Prioridad;
            tarea.Completada = dto.Completada;

            Repository.Update(tarea);

            return true;
        }

        public bool Delete(int id, int usuarioId)
        {
            var tarea = Repository.Get(id);

            if (tarea == null)
                return false;

            if (tarea.UsuarioId != usuarioId)
                return false;

            Repository.Delete(id);

            return true;
        }
    }
}
