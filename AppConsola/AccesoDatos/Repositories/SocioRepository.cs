using System;
using AccesoDatos.Data;
using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Repositories
{
    public class SocioRepository : GenericRepository<Socio>
    {
        public void ReporteHistorialPorUsuario()
        {
            var alquileres = _context.Alquiler
                .Include(a => a.Socio)
                .Include(a => a.listapeliculas)
                .ToList();

            Console.WriteLine("\n===== HISTORIAL POR USUARIO =====");

            foreach (var a in alquileres)
            {
                Console.WriteLine($"Socio: {a.Socio.Nombre} {a.Socio.Apellido}");
                Console.WriteLine($"Alquiler N° {a.Id}");
                Console.WriteLine($"Estado: {a.Estado}");
                Console.WriteLine($"Fecha: {a.Fechaentrega}");
                Console.WriteLine("---------------------------");
            }
        }
    }
}
