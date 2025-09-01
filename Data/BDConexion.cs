using Microsoft.EntityFrameworkCore;
using SistemaReciclaje.Models;

namespace SistemaReciclaje.Data
{
    public class ReciclajeDbContext : DbContext
    {
        public ReciclajeDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PuntoVerde> PuntosVerdes { get; set; }
        public DbSet<Material> Materiales { get; set; }
        public DbSet<RegistroReciclaje> RegistrosReciclaje { get; set; }
        public DbSet<Beneficio> Beneficios { get; set; }
        public DbSet<CanjeBeneficio> CanjesBeneficios { get; set; }

        // Nueva tabla para usuarios del sistema
        public DbSet<UsuarioSistema> UsuariosSistema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de enums
            modelBuilder.Entity<UsuarioSistema>()
                .Property(e => e.Rol)
                .HasConversion<int>();

            // Crear usuario administrador por defecto
            modelBuilder.Entity<UsuarioSistema>().HasData(
                new UsuarioSistema
                {
                    Id = 1,
                    NombreUsuario = "admin",
                    NombreCompleto = "Administrador del Sistema",
                    Email = "admin@reciclaje.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Rol = TipoRol.Administrador,
                    Activo = true,
                    FechaCreacion = DateTime.Now
                }
            );
        }
    }
}