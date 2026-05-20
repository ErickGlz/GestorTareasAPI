using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace GestorTareas.Models.Entities;

public partial class RegistroTareasContext : DbContext
{
    public RegistroTareasContext()
    {
    }

    public RegistroTareasContext(DbContextOptions<RegistroTareasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tareas> Tareas { get; set; }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Tareas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tareas");

            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaLimite).HasColumnType("datetime");
            entity.Property(e => e.ImagenUrl).HasMaxLength(300);
            entity.Property(e => e.Prioridad).HasMaxLength(20);
            entity.Property(e => e.Titulo).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
