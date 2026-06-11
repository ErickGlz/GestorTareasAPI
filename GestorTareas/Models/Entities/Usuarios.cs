using System;
using System.Collections.Generic;

namespace GestorTareasAPI.Models.Entities;

public partial class Usuarios
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Refreshtokens> Refreshtokens { get; set; } = new List<Refreshtokens>();
}
