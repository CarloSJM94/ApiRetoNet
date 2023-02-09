using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiRetoNet.Models
{
    public partial class DBRetoNetContext : DbContext
    {
        public DBRetoNetContext()
        {
        }

        public DBRetoNetContext(DbContextOptions<DBRetoNetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TmMovimiento> TmMovimientos { get; set; } = null!;
        public virtual DbSet<TpCliente> TpClientes { get; set; } = null!;
        public virtual DbSet<TpCuentum> TpCuenta { get; set; } = null!;
        public virtual DbSet<TpGenero> TpGeneros { get; set; } = null!;
        public virtual DbSet<TpPersona> TpPersonas { get; set; } = null!;
        public virtual DbSet<TpTipoCuentum> TpTipoCuenta { get; set; } = null!;
        public virtual DbSet<TpTipoMovimiento> TpTipoMovimientos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TmMovimiento>(entity =>
            {
                entity.ToTable("TM_Movimiento");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FechaMovimiento).HasColumnType("datetime");

                entity.Property(e => e.IdTpTipoMovimiento).HasColumnName("ID_TP_TipoMovimiento");

                entity.Property(e => e.NumeroCuenta).HasColumnName("Numero_Cuenta");

                entity.Property(e => e.Saldo).HasColumnType("money");

                entity.Property(e => e.Valor).HasColumnType("money");

                entity.HasOne(d => d.IdTpTipoMovimientoNavigation)
                    .WithMany(p => p.TmMovimientos)
                    .HasForeignKey(d => d.IdTpTipoMovimiento);

                entity.HasOne(d => d.NumeroCuentaNavigation)
                    .WithMany(p => p.TmMovimientos)
                    .HasPrincipalKey(p => p.NumeroCuenta)
                    .HasForeignKey(d => d.NumeroCuenta)
                    .HasConstraintName("FK_TM_Movimiento_TP_Cuenta_Numero_Numero_Cuenta");
            });

            modelBuilder.Entity<TpCliente>(entity =>
            {
                entity.HasKey(e => e.IdentificacionPersona)
                    .HasName("PK__TP_Clien__AF1EAD7C767C5161");

                entity.ToTable("TP_Cliente");

                entity.Property(e => e.IdentificacionPersona)
                    .ValueGeneratedNever()
                    .HasColumnName("Identificacion_Persona");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoCliente)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdentificacionPersonaNavigation)
                    .WithOne(p => p.TpCliente)
                    .HasForeignKey<TpCliente>(d => d.IdentificacionPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TpCuentum>(entity =>
            {
                entity.HasKey(e => new { e.IdentificacionPersona, e.NumeroCuenta })
                    .HasName("PK__TP_Cuent__22280AA3F0ABD127");

                entity.ToTable("TP_Cuenta");

                entity.HasIndex(e => e.NumeroCuenta, "UQ__TP_Cuent__D36A7DF89673F68C")
                    .IsUnique();

                entity.Property(e => e.IdentificacionPersona).HasColumnName("Identificacion_Persona");

                entity.Property(e => e.NumeroCuenta).HasColumnName("Numero_Cuenta");

                entity.Property(e => e.EstadoCuenta)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdTipoCuenta).HasColumnName("ID_Tipo_Cuenta");

                entity.Property(e => e.Saldo).HasColumnType("money");

                entity.HasOne(d => d.IdTipoCuentaNavigation)
                    .WithMany(p => p.TpCuenta)
                    .HasForeignKey(d => d.IdTipoCuenta);

                entity.HasOne(d => d.IdentificacionPersonaNavigation)
                    .WithMany(p => p.TpCuenta)
                    .HasForeignKey(d => d.IdentificacionPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TpGenero>(entity =>
            {
                entity.ToTable("TP_Genero");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Genero)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TpPersona>(entity =>
            {
                entity.HasKey(e => e.Identificacion)
                    .HasName("PK__TP_Perso__D6F931E456D7B4C7");

                entity.ToTable("TP_Persona");

                entity.Property(e => e.Identificacion).ValueGeneratedNever();

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdGenero).HasColumnName("ID_Genero");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGeneroNavigation)
                    .WithMany(p => p.TpPersonas)
                    .HasForeignKey(d => d.IdGenero);
            });

            modelBuilder.Entity<TpTipoCuentum>(entity =>
            {
                entity.ToTable("TP_TipoCuenta");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.TipoCuenta)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TpTipoMovimiento>(entity =>
            {
                entity.ToTable("TP_TipoMovimiento");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.TipoMovimiento)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
