using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class Promociones
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Descuento { get; set; }
        public bool Activa { get; set; }


    }
}
