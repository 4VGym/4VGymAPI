# 4VGymAPI

Esta es la entrega de la API del gimnasio. Junta los dos proyectos: la API es la de 4VGymAPI, pero ahora los datos se guardan en la base de datos de 4VGymEF con Entity Framework, en vez de en listas en memoria.

## Qué hace falta

- .NET 10
- SQL Server
- sqlcmd para cargar el script

## Cómo arrancarlo

1. En `appsettings.json` está la conexión. Yo uso `.\\SQLEXPRESS`, si tu servidor es otro hay que cambiarlo ahí.
2. Ejecuta el script. Crea la base `4vGYM` y mete los datos:

   ```bash
   sqlcmd -S ".\SQLEXPRESS" -E -C -f 65001 -i "SQL\4vGYM.sql"
   ```

3. Arranca la API:

   ```bash
   dotnet run
   ```

4. El Swagger está en `/swagger`.

El script se puede ejecutar más de una vez y no repite los datos. Mete 15 tipos de actividad, 60 monitores y 300 actividades. Son datos inventados.

## Endpoints

Son los mismos que tenía la API.

| Método | Ruta | Qué hace |
|---|---|---|
| GET | `/activity-types` | Saca los tipos de actividad |
| POST | `/activity-types` | Crea un tipo de actividad |
| GET | `/instructors` | Saca los monitores, se puede filtrar por `id` o `first_initial` |
| POST | `/instructors` | Crea un monitor |
| POST | `/activities` | Crea una actividad |

## Carpetas

| Carpeta | Qué hay |
|---|---|
| `Controllers` | Los endpoints |
| `Dtos` | Las clases que entran y salen por la API |
| `Models` | Las clases de las tablas |
| `Data` | El `GymDbContext` |
| `Migrations` | La migración de 4VGymEF |
| `Repositories` | Los repositorios, que es donde se une la API con la base de datos |
| `Validations` | Las validaciones de las fechas |
| `SQL` | El script con los datos |

## Decisiones

### ¿Modelos y DTO separados?

Sí. Los modelos tienen relaciones, por ejemplo una actividad tiene una lista de `Instructors`, pero en la API solo se pasan los ids en `instructors_id`. Si usara lo mismo para las dos cosas cambiaría el JSON y además saldrían ciclos al convertirlo. En el Swagger los nombres siguen siendo `Activity`, `ActivityType` e `Instructor`, para que sea igual que el original.

### ¿Dónde están las validaciones?

- En los DTO: campos obligatorios, aforo y fechas de la actividad.
- En los controladores: nombre, teléfono, email y foto.
- En los repositorios: lo que hay que mirar en la base de datos, como si el monitor ya existe o si existen el tipo de actividad y los monitores de una actividad.

El nombre repetido de un tipo de actividad lo comprueba el controlador, pero preguntando al repositorio.

### ¿Los POST guardan?

Sí. El enunciado decía que no hacía falta, pero guardan los datos además de validarlos.

## Versiones

Hay dos releases porque tenía dos versiones de la base de datos:

| Versión | Base | Tablas | Con qué funciona |
|---|---|---|---|
| `v1.1.0` | `4vGYM` | `Intructors` y `ActivitiesId` | Con la migración `InitialCreate7` de 4VGymEF |
| `v1.0.0` | `4VGymDB` | `Instructors` y `ActivityId` | Con una migración anterior, no con `InitialCreate7` |

La tabla `Intructors` está escrita así porque en 4VGymEF se llama así, y si la cambio deja de ser compatible.
