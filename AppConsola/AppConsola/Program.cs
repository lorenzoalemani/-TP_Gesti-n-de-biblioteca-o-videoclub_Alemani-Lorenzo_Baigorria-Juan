using AccesoDatos.Models;
using AccesoDatos.Repositories;

// 1. Instanciamos el repositorio.

IGenericRepository<Socio> socioRepository = new GenericRepository<Socio>();
IGenericRepository<Pelicula> peliculaRepository = new GenericRepository<Pelicula>();
IGenericRepository<Alquiler> alquilerRepository = new GenericRepository<Alquiler>();
IGenericRepository<Proveedor> proveedorRepository = new GenericRepository<Proveedor>();
IGenericRepository<Promociones> promocionesRepository = new GenericRepository<Promociones>();
IGenericRepository<Devolucion> devolucionRepository = new GenericRepository<Devolucion>();

bool continuar = true;


while (continuar)
{
    Console.WriteLine("=================================================");
Console.WriteLine("             GESTIÓN DE VIDEOCLUB");
Console.WriteLine("=================================================");
Console.WriteLine();

Console.WriteLine("----- GESTIÓN DE SOCIOS -----");
Console.WriteLine("1. Agregar socio (Alta)");
Console.WriteLine("2. Modificar socio");
Console.WriteLine("3. Eliminar socio (Baja)");
Console.WriteLine();

Console.WriteLine("----- GESTIÓN DE PELÍCULAS -----");
Console.WriteLine("4. Agregar película (Alta)");
Console.WriteLine("5. Modificar película");
Console.WriteLine("6. Eliminar película / Deshabilitar película");
Console.WriteLine();

Console.WriteLine("----- GESTIÓN DE ALQUILERES -----");
Console.WriteLine("7. Registrar alquiler");
Console.WriteLine("8. Registrar devolución");
Console.WriteLine();

Console.WriteLine("----- GESTIÓN DE PROVEEDORES -----");
Console.WriteLine("9. Agregar proveedor");
Console.WriteLine("10. Modificar proveedor");
Console.WriteLine("11. Eliminar proveedor");
Console.WriteLine();

Console.WriteLine("----- GESTIÓN DE PROMOCIONES -----");
Console.WriteLine("12. Agregar promoción");
Console.WriteLine("13. Modificar promoción");
Console.WriteLine("14. Eliminar promoción / Desactivar promoción");
Console.WriteLine();

Console.WriteLine("0. Salir");
Console.WriteLine();

Console.Write("Seleccione una opción: ");
string opcion = Console.ReadLine();

Console.Clear();

switch (opcion)
{
    case "1":
        AltaSocio();
        break;

    case "2":
        ModificarSocio();
        break;

    case "3":
        BajaSocio();
        break;

    case "4":
        AltaPelicula();
        break;

    case "5":
        ModificarPelicula();
        break;

    case "6":
        BajaPelicula();
        break;

    case "7":
        RegistrarAlquiler();
        break;

    case "8":
        RegistrarDevolucion();
        break;

    case "9":
        AltaProveedor();
        break;

    case "10":
        ModificarProveedor();
        break;

    case "11":
        BajaProveedor();
        break;

    case "12":
        AltaPromocion();
        break;

    case "13":
        ModficarPromocion();
        break;

    case "14":
        BajaPromocion();
        break;

    case "0":
        Console.WriteLine("¡Cerrando el sistema de videoclub!");
        continuar = false;
        break;

    default:
        Console.WriteLine("Opción no válida. Intente nuevamente.");
        break;
}



void AltaSocio()
{
    Console.Write("Ingrese el nombre del socio: ");
    string name = Console.ReadLine();
    Console.WriteLine("ingrese el apellido");
    string apellido = Console.ReadLine();
    Console.WriteLine("ingrese el dni");
    int dni = int.Parse(Console.ReadLine());

    Console.WriteLine("ingrese el telefono");
    int telefono = int.Parse(Console.ReadLine());
    var nuevoSocio = new Socio
    {
      
       Nombre = name,
       Apellido = apellido,
       Dni = dni,
       Telefono = telefono

       
    };

    socioRepository.Agregar(nuevoSocio);
    Console.WriteLine("Usuario agregado exitosamente.");

}

void ModificarSocio()
{
    var socios = socioRepository.ObtenerTodos();
    foreach(var s in socios)
    {
        Console.WriteLine($"ID: {s.Id} ||| NOMBRE: {s.Nombre} ||| APELLIDO: {s.Apellido}");
    }
    Console.WriteLine("ingrese el id del socio que quiere modificar");
    int id = int.Parse(Console.ReadLine());
    var socio = socioRepository.ObtenerPorId(id);
    if(socio != null)
    {
        Console.WriteLine("ingrese el nuevo nombre");
        string name = Console.ReadLine();
        Console.WriteLine("ingrese el nuevo apellido");
        string apellido = Console.ReadLine();
        Console.WriteLine("ingrese el nuevo dni");
        int dni = int.Parse(Console.ReadLine());
        Console.WriteLine("ingrese el nuevo telefono");
        int telefono = int.Parse(Console.ReadLine());
        socio.Nombre = name;
        socio.Apellido = apellido;
        socio.Dni = dni;
        socio.Telefono = telefono;
        socioRepository.Modificar(socio);
        Console.WriteLine("socio modificado exitosamente");
    }
    else
    {
        Console.WriteLine("No se encontró ningún usuario con ese ID.");
    }
}

void BajaSocio()
{
    var socios = socioRepository.ObtenerTodos();
    foreach (var s in socios)
    {
        Console.WriteLine($"ID: {s.Id} ||| NOMBRE: {s.Nombre} ||| APELLIDO: {s.Apellido}");
    }
    Console.WriteLine("ingrese el id del socio que quiere eliminar");
    int id = int.Parse(Console.ReadLine());
    var socio = socioRepository.ObtenerPorId(id);
    if (socio != null)
    {
        socioRepository.Eliminar(socio);
        Console.WriteLine("socio eliminado exitosamente");
    }
    else
    {
        Console.WriteLine("No se encontró ningún usuario con ese ID.");
    }
}
void AltaPelicula()
{
    Console.WriteLine("ingrese el titulo de la pelicula");
    string titulo = Console.ReadLine();

    

    Console.WriteLine("ingrese el autor");
    string autor = Console.ReadLine();

   Console.WriteLine("ingrese la cantidad q hay disponibles");
   int cantidad = int.Parse(Console.ReadLine());
    

        var nuevoPelicula = new Pelicula
    {
       
        Titulo = titulo,
        Autor = autor,
        Cantidad = cantidad,
        Disponible = true,
    };

    peliculaRepository.Agregar(nuevoPelicula);
    Console.WriteLine("Pelicula agregado exitosamente.");
    var proveedores = proveedorRepository.ObtenerTodos();
    foreach (var p in proveedores)
    {
        Console.WriteLine($"ID: {p.Id} ||| NOMBRE: {p.Nombre} ||| DIRECCION: {p.Direccion}");
    }
    Console.WriteLine("ingrese el id del proveedor de la pelicula");
    int id = int.Parse(Console.ReadLine());
    var proveedor = proveedorRepository.ObtenerPorId(id);
    if(proveedor != null)
    {
        proveedor.Peliculas.Add(nuevoPelicula);
        proveedorRepository.Modificar(proveedor);
        Console.WriteLine("pelicula agregada al proveedor exitosamente");

    }
    else
    {
        Console.WriteLine("No se encontró ningún proveedor con ese ID.");   
    }
}
void ModificarPelicula()
{
    var peliculas = peliculaRepository.ObtenerTodos();
    foreach (var p in peliculas)
    {
        Console.WriteLine($"ID: {p.Id} ||| NOMBRE: {p.Titulo} ||| AUTOR: {p.Autor}");
    }
    Console.WriteLine("ingrese el id de la pelicula que quiere modificar");
    int id = int.Parse(Console.ReadLine());
    var pelicula = peliculaRepository.ObtenerPorId(id);
    if (pelicula != null)
    {
        Console.WriteLine("ingrese el nuevo titulo");
        string titulo = Console.ReadLine();
        Console.WriteLine("ingrese el nuevo autor");
        string autor = Console.ReadLine();
        Console.WriteLine("ingrese la nueva cantidad");
        int cantidad = int.Parse(Console.ReadLine());
        Console.WriteLine("ingrese si esta disponible o no (true/false)");
        bool disponible = bool.Parse(Console.ReadLine());
        pelicula.Titulo = titulo;
        pelicula.Autor = autor;
        pelicula.Cantidad = cantidad;
        pelicula.Disponible = disponible;
        peliculaRepository.Modificar(pelicula);
        Console.WriteLine("pelicula modificada exitosamente");
    }
    else
    {
        Console.WriteLine("No se encontró ninguna pelicula con ese ID.");
    }
}

void BajaPelicula()
{
    var pelis = peliculaRepository.ObtenerTodos();
    foreach (var p in pelis)
    {
        Console.WriteLine($"ID: {p.Id} ||| NOMBRE: {p.Titulo} ||| AUTOR: {p.Autor}");
    }
    Console.WriteLine("ingrese el id de la pelicula");
    int id = int.Parse(Console.ReadLine());
    var pelicula = peliculaRepository.ObtenerPorId(id);
    Console.WriteLine("ingrese que quiere hacer: 1 -> QUE NO ESTE DISPONIBLE ||| 2 -> ELIMINAR");
    int opcion = int.Parse(Console.ReadLine());
    if (opcion == 1)
    {
        pelicula.Disponible = false;
        peliculaRepository.Modificar(pelicula);
        Console.WriteLine("pelicula modificada exitosamente");
    }
    else if (opcion == 2)
    {
        peliculaRepository.Eliminar(pelicula);
        Console.WriteLine("pelicula eliminada exitosamente");
    }
}

void AltaProveedor()
{
    Console.WriteLine("ingrese el nombre del proveedor");
    string nombre = Console.ReadLine();
    Console.WriteLine("ingrese la direccion del proveedor");
    string direccion = Console.ReadLine();
    var nuevoProveedor = new Proveedor
    {
        Nombre = nombre,
        Direccion = direccion
    };
    proveedorRepository.Agregar(nuevoProveedor);
    Console.WriteLine("Proveedor agregado exitosamente.");

}
void ModificarProveedor()
    {
        var proveedores = proveedorRepository.ObtenerTodos();
        foreach (var p in proveedores)
        {
            Console.WriteLine($"ID: {p.Id} ||| NOMBRE: {p.Nombre} ||| DIRECCION: {p.Direccion}");
        }
        Console.WriteLine("ingrese el id del proveedor que quiere modificar");
        int id = int.Parse(Console.ReadLine());
        var proveedor = proveedorRepository.ObtenerPorId(id);
        if (proveedor != null)
        {
            Console.WriteLine("ingrese el nuevo nombre");
            string nombre = Console.ReadLine();
            Console.WriteLine("ingrese la nueva direccion");
            string direccion = Console.ReadLine();
            proveedor.Nombre = nombre;
            proveedor.Direccion = direccion;
            proveedorRepository.Modificar(proveedor);
            Console.WriteLine("proveedor modificado exitosamente");
        }
        else
        {
            Console.WriteLine("No se encontró ningún proveedor con ese ID.");
        }
    }
    void BajaProveedor()
    {
        var proveedores = proveedorRepository.ObtenerTodos();
        foreach (var p in proveedores)
        {
            Console.WriteLine($"ID: {p.Id} ||| NOMBRE: {p.Nombre} ||| DIRECCION: {p.Direccion}");
        }
        Console.WriteLine("ingrese el id del proveedor que quiere eliminar");
        int id = int.Parse(Console.ReadLine());
        var proveedor = proveedorRepository.ObtenerPorId(id);
        if (proveedor != null)
        {
            proveedorRepository.Eliminar(proveedor);
            Console.WriteLine("proveedor eliminado exitosamente");
        }
        else
        {
            Console.WriteLine("No se encontró ningún proveedor con ese ID.");
        }
    }

    void AltaPromocion()
    {
        Console.WriteLine("ingrese el nombre de la promocion");
        string nombre = Console.ReadLine();
        Console.WriteLine("ingrese el descuento de la promocion");
        decimal descuento = decimal.Parse(Console.ReadLine());
      
        var nuevaPromocion = new Promociones
        {
            Nombre = nombre,
            Descuento = descuento,
            Activa = true
        };
        promocionesRepository.Agregar(nuevaPromocion);
        Console.WriteLine("Promocion agregada exitosamente.");
    }
    void ModficarPromocion()
    {
        var promociones = promocionesRepository.ObtenerTodos();
        foreach (var p in promociones)
        {
            Console.WriteLine($"ID: {p.Id} ||| NOMBRE: {p.Nombre} ||| DESCUENTO: {p.Descuento}");
        }
        Console.WriteLine("ingrese el id de la promocion que quiere modificar");
        int id = int.Parse(Console.ReadLine());
        var promocion = promocionesRepository.ObtenerPorId(id);
        if (promocion != null)
        {
            Console.WriteLine("ingrese el nuevo nombre");
            string nombre = Console.ReadLine();
            Console.WriteLine("ingrese el nuevo descuento");
            decimal descuento = decimal.Parse(Console.ReadLine());
            promocion.Nombre = nombre;
            promocion.Descuento = descuento;
            promocionesRepository.Modificar(promocion);
            Console.WriteLine("promocion modificada exitosamente");
        }
        else
        {
            Console.WriteLine("No se encontró ninguna promocion con ese ID.");
        }
    }
    void BajaPromocion()
    {
        var promociones = promocionesRepository.ObtenerTodos();
        foreach (var p in promociones)
        {
            Console.WriteLine($"ID: {p.Id} ||| NOMBRE: {p.Nombre} ||| DESCUENTO: {p.Descuento}");
        }
        Console.WriteLine("ingrese el id de la promocion que quiere eliminar");
        int id = int.Parse(Console.ReadLine());
        var promocion = promocionesRepository.ObtenerPorId(id);
        if (promocion != null)
        {
            Console.WriteLine("ingrese que quiere hacer: 1 -> QUE NO ESTE ACTIVA ||| 2 -> ELIMINAR");
            int opcion = int.Parse(Console.ReadLine());
            if (opcion == 1)
            {
                promocion.Activa = false;
                promocionesRepository.Modificar(promocion);
                Console.WriteLine("promocion modificada exitosamente");
            }
            else if (opcion == 2)
            {
                promocionesRepository.Eliminar(promocion);
                Console.WriteLine("promocion eliminada exitosamente");
            }
        }
        
    }

    void RegistrarAlquiler()
    {
        var listasocios = socioRepository.ObtenerTodos();
        foreach (var a in listasocios)
        {
            Console.WriteLine($"ID :{a.Id} ||| NOMBRE:{a.Nombre} ||| APELLIDO: {a.Apellido}");
        }
        Console.WriteLine("ingrese el id del socio que quieres registrar el alquiler");
        int id = int.Parse(Console.ReadLine());

        var socio = socioRepository.ObtenerPorId(id);
        if (socio != null)
        {
            Console.WriteLine("socio encontrado con exito");
        }
        else
        {
            Console.WriteLine("No se encontró ningún usuario con ese ID.");
        }

        var listapelis = peliculaRepository.ObtenerTodos();
        Console.WriteLine("ingrese cuantas pelis quiere alquilar");
        int cant = int.Parse(Console.ReadLine());

     

        List<Pelicula> peliculasseleccionadas = new List<Pelicula>();
        for (int i = 0; i < cant; i++)
        {
            foreach (var a in listapelis)
            {
                Console.WriteLine($"ID :{a.Id} ||| NOMBRE:{a.Titulo} |||   {a.Autor}");
            }
            Console.WriteLine("ingrese el id de la peli que quiere alquilar");
            int idp = int.Parse(Console.ReadLine());
            var peli = peliculaRepository.ObtenerPorId(idp);
            if (peli.Cantidad > 0)
            {
                peliculasseleccionadas.Add(peli);
                peli.Cantidad--;
                peliculaRepository.Modificar(peli);
            }
            else
            {
                Console.WriteLine("no se puede agregar la peli porque no hay cantidad suficiente");
                i--;
            }



        }
        Console.WriteLine("ingrese el monto");
        decimal monto = decimal.Parse(Console.ReadLine());
        Console.WriteLine("desea que el alquiler tenga una promocion? 1 -> SI ||| 2 -> NO");
        int opcion = int.Parse(Console.ReadLine());
        if (opcion == 1)
        {
            var promociones = promocionesRepository.ObtenerTodos();
            foreach (var p in promociones)
            {
                Console.WriteLine($"ID: {p.Id} ||| NOMBRE: {p.Nombre} ||| DESCUENTO: {p.Descuento}");
            }
            Console.WriteLine("ingrese el id de la promocion que quiere aplicar");
            int idpromocion = int.Parse(Console.ReadLine());
            var promocion = promocionesRepository.ObtenerPorId(idpromocion);
            monto = monto - (monto * promocion.Descuento / 100);

        }
        else
        {
            Console.WriteLine("no se aplicara ninguna promocion");
        }

        
        if (peliculasseleccionadas.Count > 0)
        {
            var nuevoalquiler = new Alquiler
            {
                Socioid = id,
                Fechaentrega = DateTime.Now,
                Fechadevolucion = DateTime.Now.AddDays(7),
                listapeliculas = peliculasseleccionadas,
                Monto = monto,
                Estado = "Activo"

            };
            alquilerRepository.Agregar(nuevoalquiler);
            socio.Alquileres.Add(nuevoalquiler);
            socioRepository.Modificar(socio);
            
        }
        else
        {
            Console.WriteLine("no se puede registrar el alquiler porque no hay peliculas seleccionadas");
        }
    }
void RegistrarDevolucion()
    {
        var listaalquileres = alquilerRepository.ObtenerTodos();
        foreach (var a in listaalquileres)
        {
            Console.WriteLine($"ID :{a.Id} ||| SOCIO ID:{a.Socioid} ||| FECHA ENTREGA: {a.Fechaentrega}");
        }
        Console.WriteLine("ingrese el id del alquiler que quiere devolver");
        int id = int.Parse(Console.ReadLine());
        var alquiler = alquilerRepository.ObtenerPorId(id);
        if (alquiler != null)
        {
            foreach (var p in alquiler.listapeliculas)
            {
                p.Cantidad++;
                peliculaRepository.Modificar(p);
            }
            alquiler.Fechadevolucion = DateTime.Now;
            alquiler.Estado = "Devuelto";
            alquilerRepository.Modificar(alquiler);
            Console.WriteLine("devolucion registrada exitosamente");
        }
        else
        {
            Console.WriteLine("No se encontró ningún alquiler con ese ID.");
        }
    }
}

