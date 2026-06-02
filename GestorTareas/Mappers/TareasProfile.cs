using AutoMapper;
using GestorTareas.DTOs.Tareas;
using GestorTareasAPI.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestorTareasAPI.Mappers
{
    public class TareasProfile : Profile
    {
        public TareasProfile()
        {
            CreateMap<Tareas, TareaDTO>();

            CreateMap<CrearTareaDTO, Tareas>();

            CreateMap<EditarTareaDTO, Tareas>();
        }
    }
}
