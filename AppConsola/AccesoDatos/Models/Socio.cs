using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public  class Socio 

    {
       public int Id { get; set; }
        public string Nombre { get; set; }
       public string Apellido { get; set; }
        public int Dni {  get; set; }
        public int Telefono { get; set; }
        
        public List<Alquiler> Alquileres { get; set; }

    }
}
