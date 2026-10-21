using AccesoDatos.Data;
using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Repositories
{
    public class AlquilerRepository : GenericRepository<Alquiler>
    {
        private readonly AplicationDbContext _context = new AplicationDbContext();

        public void ReporteAlquileresActivosYVencidos()
        {
            var alquileres = _context.Alquiler
                .Include(a => a.Socio)
                .Where(a => a.Estado == "Activo")
                .ToList();

            Console.WriteLine("\n===== ALQUILERES ACTIVOS Y VENCIDOS =====");

            foreach (var a in alquileres)
            {
                string situacion;
                if (a.Fechadevolucion < DateTime.Now)
                {
                    situacion = "VENCIDO";
                }
                else
                {
                    situacion = "ACTIVO";
                }

                Console.WriteLine($"Alquiler N° {a.Id}");
                Console.WriteLine($"Socio: {a.Socio.Nombre} {a.Socio.Apellido}");
                Console.WriteLine($"Fecha de entrega: {a.Fechaentrega}");
                Console.WriteLine($"Fecha de devolución: {a.Fechadevolucion}");
                Console.WriteLine($"Situación: {situacion}");
                Console.WriteLine("---------------------------");
            }
        }
    }
}