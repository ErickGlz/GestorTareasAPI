namespace GestorTareasAPI.Models.DTOs
{
    public class TareaDTO
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public DateTime FechaLimite { get; set; }

        public string Prioridad { get; set; } = null!;

        public bool Completada { get; set; }


        public DateTime FechaCreacion { get; set; }
    }

    public class CrearTareaDTO
    {
        public string Titulo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public DateTime FechaLimite { get; set; }

        public string Prioridad { get; set; } = null!;

    }

    public class EditarTareaDTO
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public DateTime FechaLimite { get; set; }

        public string Prioridad { get; set; } = null!;

        public bool Completada { get; set; }

    }
}
