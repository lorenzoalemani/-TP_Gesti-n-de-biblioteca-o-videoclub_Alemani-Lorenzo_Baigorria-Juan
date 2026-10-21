using System;
using AccesoDatos.Data;
using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Repositories
{
    public class PromocionesRepository : GenericRepository<Promociones>
    {
        public void ReportePromocionesMasUtilizadas()
        {
            // 1. Traigo todas las promociones
            var promociones = _context.Promociones.ToList();

            Console.WriteLine("\n===== PROMOCIONES MÁS UTILIZADAS =====");

            foreach (var promo in promociones)
            {
                // 2. Cuento cuántos alquileres usaron esta promoción
                int veces = _context.Alquiler
                    .Count(a => a.PromocionId == promo.Id);

                Console.WriteLine($"Promoción: {promo.Nombre}");
                Console.WriteLine($"Descuento: {promo.Descuento}%");
                Console.WriteLine($"Veces usada: {veces}");
                Console.WriteLine("---------------------------");
            }
        }

    }
}
