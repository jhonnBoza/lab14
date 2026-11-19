using Gimnasio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos;

public class GimnasioDbContext : DbContext
{
    public GimnasioDbContext(DbContextOptions<GimnasioDbContext> options) : base(options)
    {
    }

    public DbSet<Plan> Planes => Set<Plan>();

    public DbSet<Miembro> Miembros => Set<Miembro>();

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasIndex(p => p.Nombre).IsUnique();
            entity.Property(p => p.Nombre).HasMaxLength(80);
            entity.Property(p => p.Descripcion).HasMaxLength(300);
        });

        modelBuilder.Entity<Miembro>(entity =>
        {
            entity.HasIndex(m => m.Email).IsUnique();
            entity.Property(m => m.Nombres).HasMaxLength(80);
            entity.Property(m => m.Apellidos).HasMaxLength(80);
            entity.Property(m => m.Email).HasMaxLength(120);
            entity.Property(m => m.Telefono).HasMaxLength(20);
            entity.HasOne(m => m.Plan)
                .WithMany(p => p.Miembros)
                .HasForeignKey(m => m.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Miembro_Plan");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Correo).IsUnique();
            entity.Property(u => u.Correo).HasMaxLength(120);
            entity.Property(u => u.ClaveHash).HasMaxLength(200);
            entity.Property(u => u.Rol).HasMaxLength(40);
        });
    }
}
