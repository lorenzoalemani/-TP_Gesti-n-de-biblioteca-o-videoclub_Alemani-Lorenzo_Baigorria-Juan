using System;
using AccesoDatos.Data;
using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Repositories
{
    public class ProveedorRepository : GenericRepository<Proveedor>
    {
        public void ReporteStockPorProveedor()
        {
            // 1. Traigo todos los proveedores con sus películas
            var proveedores = _context.Proveedor
                .Include(p => p.Peliculas)
                .ToList();

            Console.WriteLine("\n===== STOCK POR PROVEEDOR =====");

            foreach (var proveedor in proveedores)
            {
                int totalPeliculas = proveedor.Peliculas.Count;
                int stockTotal = 0;

                // 2. Sumo la cantidad de cada película
                foreach (var pelicula in proveedor.Peliculas)
                {
                    stockTotal = stockTotal + pelicula.Cantidad;
                }

                Console.WriteLine($"Proveedor: {proveedor.Nombre}");
                Console.WriteLine($"Cantidad de títulos: {totalPeliculas}");
                Console.WriteLine($"Stock total: {stockTotal}");
                Console.WriteLine("---------------------------");
            }
        }
    }
}
