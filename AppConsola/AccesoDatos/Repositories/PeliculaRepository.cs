using System;
using AccesoDatos.Data;
using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Repositories
{
    public class PeliculaRepository : GenericRepository<Pelicula>
    {
        private readonly AplicationDbContext _context = new AplicationDbContext();

        public void ReportePeliculasMasAlquiladas()
        {
            // 1. Traigo todos los alquileres con sus películas
            var alquileres = _context.Alquiler
                .Include(a => a.listapeliculas)
                .ToList();

            // 2. Junto todas las películas de todos los alquileres en una sola lista
            var todasLasPeliculas = new List<string>();

            foreach (var alquiler in alquileres)
            {
                foreach (var pelicula in alquiler.listapeliculas)
                {
                    todasLasPeliculas.Add(pelicula.Titulo);
                }
            }

            // 3. Cuento cuántas veces aparece cada título
            var ranking = todasLasPeliculas
                .GroupBy(titulo => titulo)
                .OrderByDescending(g => g.Count())
                .Take(10);

            // 4. Muestro el resultado
            Console.WriteLine("\n===== PELÍCULAS MÁS ALQUILADAS =====");
            int puesto = 1;
            foreach (var grupo in ranking)
            {
                Console.WriteLine($"{puesto}. {grupo.Key} → {grupo.Count()} veces");
                puesto++;
            }
        }
    }
}
