# 4VGymAPI

API REST para gestionar las actividades, los tipos de actividad y los monitores del gimnasio 4VGym. Une dos proyectos: mantiene la definición de la API de 4VGymAPI y guarda los datos en la base de datos SQL Server de 4VGymEF, mediante Entity Framework.

## Requisitos

- .NET 10
- SQL Server
- Herramienta `sqlcmd`, para cargar el script de datos

## Puesta en marcha

1. Revisa la cadena de conexión `DefaultConnection` en `appsettings.json` y cambia el servidor `.\\SQLEXPRESS` por el tuyo si es distinto.
2. Crea la base de datos `4vGYM` y carga los datos de ejemplo:

   ```bash
   sqlcmd -S ".\SQLEXPRESS" -E -C -f 65001 -i "SQL\4vGYM.sql"
   ```

3. Arranca la API:

   ```bash
   dotnet run
   ```

4. En desarrollo, la documentación está en `/swagger` y la especificación OpenAPI en `/openapi/v1.json`.

El script se puede ejecutar más de una vez sin duplicar datos. Carga 15 tipos de actividad, 60 monitores, 300 actividades y sus asignaciones de monitores, todos ficticios.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/activity-types` | Lista los tipos de actividad |
| POST | `/activity-types` | Crea un tipo de actividad |
| GET | `/instructors` | Lista los monitores; admite `id` y `first_initial` |
| POST | `/instructors` | Crea un monitor |
| POST | `/activities` | Crea una actividad |

## Estructura

| Carpeta | Contenido |
|---|---|
| `Controllers` | Reciben la petición y devuelven la respuesta HTTP |
| `Dtos` | Objetos de entrada y salida de la API |
| `Models` | Entidades que Entity Framework mapea a las tablas |
| `Data` | `GymDbContext` |
| `Migrations` | Definición de la base de datos |
| `Repositories` | Acceso a los datos; son la unión entre la API y la base de datos |
| `Validations` | Atributos de validación propios |
| `SQL` | Script con los datos de ejemplo |

## Decisiones

### Separar los modelos de base de datos y los DTO

Sí. Las entidades de `Models` tienen propiedades de navegación, como `Instructors`, mientras que la API trabaja con identificadores, como `instructors_id`. Separarlas mantiene el mismo JSON que la API original y evita los ciclos al serializar. El documento OpenAPI conserva los nombres originales de los esquemas: `Activity`, `ActivityType` e `Instructor`.

### Dónde se hacen las validaciones

- **DTO:** las reglas sobre un solo objeto que se expresan con atributos: campos obligatorios, aforo y fechas de la actividad.
- **Controladores:** las validaciones de entrada de tipos de actividad y monitores: nombre, teléfono, email y URL de la foto.
- **Repositorios:** las reglas que necesitan consultar la base de datos: duplicados de monitores y existencia del tipo de actividad y de los monitores. La comprobación de nombre repetido de tipo de actividad la decide el controlador, consultando al repositorio.

### Inserciones en los POST

El enunciado no exigía insertar en la base de datos, pero los POST sí guardan los datos además de validarlos, con `Add` y `SaveChangesAsync`.

## Versiones y base de datos

Hay dos versiones publicadas, según la base de datos que se quiera usar:

| Versión | Base de datos | Compatible con |
|---|---|---|
| `v1.1.0` | `4vGYM`, tablas `Intructors` y `ActivityInstructor` con `ActivitiesId` | La migración `InitialCreate7` de 4VGymEF |
| `v1.0.0` | `4VGymDB`, tablas `Instructors` y `ActivityInstructor` con `ActivityId` | Una versión anterior de la migración; no funciona con una base creada con `InitialCreate7` |

La tabla `Intructors` conserva el nombre con el que la define 4VGymEF para mantener la compatibilidad.
