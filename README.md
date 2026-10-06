# -TP_Gesti-n-de-biblioteca-o-videoclub_Alemani-Lorenzo_Baigorria-Juan
# TP_GestionBiblioteca_Grupo[Número]

## Descripción General del Sistema
Este sistema está diseñado para la **Gestión  de unVideoclub**, para el control del catálogo de recursos multimedia (películas) y el seguimiento de los préstamos realizados a los socios. 

### Entidades Principales
- **Recurso / Item (Película):** Representa el stock disponible en el catálogo con sus datos específicos (título, autor/director, género, código de identificación, stock).
- **Socio / Usuario:** Persona registrada en el sistema habilitada para solicitar préstamos.
- **Alquiler:** Registro de la transacción entre un socio y una pelicula, incluyendo fechas de inicio, devolución pactada y devolución real.
- **Devolucion:** Registro del retorno de las películas alquiladas, evaluando la fecha efectiva de entrega, posibles recargos por mora o el estado de la pelicula devuelta
- **Proveedor:** Empresa o entidad que suministra las películas (copias físicas o licencias) al videoclub.
- **Promocion:** Descuentos, ofertas especiales o beneficios aplicables a los alquileres
---

## Objetivos y Funcionalidades Previstas

### Funcionalidades ABM (Alta, Baja y Modificación)
1. **ABM de Peliculas:**
   - **Alta:** Registrar una nueva película en el catálogo con sus atributos correspondientes.
   - **Modificación:** Actualizar información técnica, categoría o cantidad de ejemplares disponibles.
   - **Baja:** Dar de baja lógica a un recurso que esté descatalogado o fuera de circulación.

2. **ABM de Socios:**
   - **Alta:** Registrar nuevos usuarios completando sus datos personales y de contacto.
   - **Modificación:** Actualizar estado de membresía, domicilio o número telefónico.
   - **Baja:** Suspender o eliminar el registro de un socio del sistema.

3. **Gestión de Préstamos y Devoluciones (Operativa Principal):**
   - Registrar la salida de un recurso hacia un socio.
   - Asignar fecha límite de devolución.
   - Procesar el retorno del recurso y calcular sanciones si aplica.
     
4. **ABM de Proveedores:**
   - **Alta:** Registrar nuevos distribuidores o proveedores con sus datos de contacto (CUIT, Razón Social, teléfono, email).
   - **Modificación:** Actualizar datos institucionales o condiciones de suministro.
   - **Baja:** Inhabilitar a un proveedor con el que ya no se trabaja.

5. **ABM de Promociones:**
   - **Alta:** Crear promociones indicando porcentaje/monto de descuento, días de vigencia y condiciones de aplicación.
   - **Modificación:** Ajustar porcentaje de descuento, fechas límite o condiciones de la oferta.
   - **Baja:** Desactivar una promoción finalizada o vencida.
---

### Reportes Previstos
1. **Reporte de Alquileres Activos y Vencidos:** Listado de las películas actualmente alquiladas, indicando el usuario responsable y destacando las entregas fuera de término.
2. **Reporte de Películas Más Alquiladas (Top 10):** Ranking de los títulos más populares en un rango de fechas.
3. **Reporte de Promociones Más Utilizadas:** Métricas sobre cuántas veces se aplicó cada promoción.
4. **Reporte de Compras/Stock por Proveedor:** Balance de películas adquiridas a cada proveedor, stock actual provisto y costos de adquisición.
5. **Reporte de Historial por Usuario:** Detalle de alquileres realizados, devoluciones a tiempo y promociones aprovechadas por un cliente específico.

---

## Flujo de Integración por Capas (Guardar un Registro en la Base de Datos)

Para guardar un nuevo registro (por ejemplo, el **Alta de un alquiler**), el sistema procesa la solicitud a través de la arquitectura en capas de la siguiente manera:

1. **Capa de Presentación (UI / Frontend):**
   - El usuario (bibliotecario) completa el formulario con el ID del socio y el ID de la pelicula.
   - La UI realiza una validación básica de formato (campos no vacíos, tipos de datos correctos) y envía un objeto DTO a la capa de lógica.

2. **Capa de Negocio / Servicios (Backend Logic):**
   - Recibe la solicitud y aplica las reglas de negocio:
     - Verifica si el socio no tiene multas pendientes o exceso de préstamos activos.
     - Verifica si el ejemplar está disponible.
   - Si se cumplen las condiciones, instancia el objeto `Prestamo` con sus fechas correspondientes y llama al repositorio/DAO.

3. **Capa de Acceso a Datos (DAO / Repositorio / ORM):**
   - Convierte el objeto `Prestamo` en una entidad ejecutable para la base de datos (mediante sentencias SQL `INSERT` directas o a través de un ORM como Entity Framework / Hibernate).
   - Administra la transacción con el motor de base de datos.

4. **Capa de Persistencia (Base de Datos):**
   - La BD ejecuta la inserción, genera la clave primaria correspondiente y confirma la transacción (`COMMIT`).
   - El resultado retorna en sentido inverso hacia la UI confirmando el alta exitosa.
