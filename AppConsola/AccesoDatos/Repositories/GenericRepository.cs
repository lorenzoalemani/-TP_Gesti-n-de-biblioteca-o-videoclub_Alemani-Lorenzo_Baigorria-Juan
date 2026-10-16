using AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;

namespace AccesoDatos.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AplicationDbContext _context;

        public GenericRepository()
        {
            _context = new AplicationDbContext();
        }

        // 1. LECTURA (SELECT *)
        public List<T> ObtenerTodos()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        // NUEVO MÉTODO CON INCLUDE
        public List<T> ObtenerTodosCon(string propiedadRelacionada)
        {
            return _context.Set<T>()
                .Include(propiedadRelacionada)
                .AsNoTracking()
                .ToList();
        }

        // 2. ALTA (INSERT)
        public void Agregar(T entidad)
        {
            _context.Set<T>().Add(entidad);
             _context.SaveChanges();
        }

        // 3. BAJA (DELETE)
        public void Eliminar(object id)
        {
            var entidad = _context.Set<T>().Find(id);
            if (entidad != null)
            {
                _context.Set<T>().Remove(entidad);
                _context.SaveChanges();
            }
        }

        // 4. MODIFICACIÓN (UPDATE)
        public void Modificar(T entidad)
        {
            _context.Set<T>().Update(entidad);
            _context.SaveChanges();
        }

        // 5. BÚSQUEDA POR ID
        public T ObtenerPorId(int id)
        {
            return _context.Set<T>().Find(id);
        }

    }

}