using System;
using System.Collections.Generic;

namespace GestorTareasAPI.Models.Entities;

public partial class Tareas
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaLimite { get; set; }

    public string Prioridad { get; set; } = null!;

    public bool Completada { get; set; }

    public string? ImagenUrl { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int UsuarioId { get; set; }
}
