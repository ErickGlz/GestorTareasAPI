using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTareasApp.Models
{
    public class TareaModel
    {
        public int Id { get; set; }

        public string? Titulo { get; set; }

        public string? Descripcion { get; set; }

        public DateTime FechaLimite { get; set; }

        public string? Prioridad { get; set; }

        public bool Completada { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string? ImagenUrl { get; set; }
    }
}
