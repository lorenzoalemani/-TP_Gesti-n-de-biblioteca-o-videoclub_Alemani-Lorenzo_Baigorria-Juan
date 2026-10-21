using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class Devolucion
    {
        public int Id { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public int AlquilerId { get; set; }
        public Alquiler Alquiler { get; set; }

    }
}
