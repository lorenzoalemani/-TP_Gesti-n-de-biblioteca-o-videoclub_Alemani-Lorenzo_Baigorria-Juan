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
            optionsBuilder.UseSqlite("Data Source=C:\\databases\\exampleDB.db");
        }
    }
}
