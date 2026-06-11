using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace GestorTareasAPI.Models.Entities;

public partial class RegistrotareasContext : DbContext
{
    public RegistrotareasContext()
    {
    }

    public RegistrotareasContext(DbContextOptions<RegistrotareasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Refreshtokens> Refreshtokens { get; set; }

    public virtual DbSet<Tareas> Tareas { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Refreshtokens>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("refreshtokens");

            entity.HasIndex(e => e.UsuarioId, "FK_RefreshTokens_Usuarios");

            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Refreshtokens)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_RefreshTokens_Usuarios");
        });

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

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
