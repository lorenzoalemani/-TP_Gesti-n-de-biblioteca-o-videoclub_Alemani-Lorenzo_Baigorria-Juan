using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public  class Alquiler
    {
        public int Id { get; set; }
        public int Socioid { get; set; }
        public DateTime Fechaentrega { get; set; }
        public DateTime Fechadevolucion { get; set; }
        public decimal Monto { get; set; }
        public List<Pelicula> listapeliculas { get; set; }
        public Socio Socio { get; set; }
        public string Estado { get; set; }
        public Promociones Promocion { get; set; }
        public int? PromocionId { get; set; }


    }
}
