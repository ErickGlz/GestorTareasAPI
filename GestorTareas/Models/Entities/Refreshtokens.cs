using System;
using System.Collections.Generic;

namespace GestorTareasAPI.Models.Entities;

public partial class Refreshtokens
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expiration { get; set; }

    public bool Revoked { get; set; }

    public virtual Usuarios Usuario { get; set; } = null!;
}
