using Microsoft.EntityFrameworkCore;
using AccesoDatos.Models;

namespace AccesoDatos.Data
{
    public class AplicationDbContext : DbContext
    {
       
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Socio> Socio { get; set; }
        public DbSet<Alquiler> Alquiler { get; set; }
        public DbSet<Promociones> Promociones { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Devolucion> Devolucion { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string carpeta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "VideoclubAlemaniBaigorria"
            );

            Directory.CreateDirectory(carpeta);

            string rutaBD = Path.Combine(carpeta, "videoclub.db");

            optionsBuilder.UseSqlite($"Data Source={rutaBD}");
        }

    }
}
