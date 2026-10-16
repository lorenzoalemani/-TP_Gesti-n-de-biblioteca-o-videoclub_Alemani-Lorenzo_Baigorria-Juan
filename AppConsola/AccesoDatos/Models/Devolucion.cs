using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class Devolucion
    {
        public int Id { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string Descripcion { get; set; }
    }
}
