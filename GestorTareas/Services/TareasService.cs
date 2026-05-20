using AutoMapper;
using GestorTareas.DTOs.Tareas;
using GestorTareas.Models.Entities;
using GestorTareas.Repositories;

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

        public IEnumerable<TareaDTO> GetAll()
        {
            var tareas = Repository.GetAll();

            return Mapper.Map<IEnumerable<TareaDTO>>(tareas);
        }

        public TareaDTO? GetById(int id)
        {
            var tarea = Repository.Get(id);

            if (tarea == null)
                return null;

            return Mapper.Map<TareaDTO>(tarea);
        }

        public void Insert(CrearTareaDTO dto)
        {
            var tarea = Mapper.Map<Tareas>(dto);

            tarea.FechaCreacion = DateTime.Now;

            tarea.Completada = false;

            Repository.Insert(tarea);
        }

        public bool Update(EditarTareaDTO dto)
        {
            var tarea = Repository.Get(dto.Id);

            if (tarea == null)
                return false;

            tarea.Titulo = dto.Titulo;
            tarea.Descripcion = dto.Descripcion;
            tarea.FechaLimite = dto.FechaLimite;
            tarea.Prioridad = dto.Prioridad;
            tarea.Completada = dto.Completada;
            tarea.ImagenUrl = dto.ImagenUrl;

            Repository.Update(tarea);

            return true;
        }

        public bool Delete(int id)
        {
            var tarea = Repository.Get(id);

            if (tarea == null)
                return false;

            Repository.Delete(id);

            return true;
        }
    }
}
