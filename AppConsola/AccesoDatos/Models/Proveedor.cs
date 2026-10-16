using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class Proveedor
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public string Direccion { get; set; }
        public List<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
    }
}
