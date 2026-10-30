using _4VGymAPI.Models;
using _4VGymAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _4VGymAPI.Controllers
{
    [ApiController]
    [Route("activity-types")]
    public class ActivityTypesController : ControllerBase
    {
        private readonly InMemoryActivityTypeRepository _repository;

        // Inyectamos la clase concreta del repositorio
        public ActivityTypesController(InMemoryActivityTypeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// GET /activity-types
        /// Devuelve el listado de tipos de actividades de 4VGym.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ActivityType>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<ActivityType>> FindActivityTypes()
        {
            try
            {
                IEnumerable<ActivityType> activityTypes = _repository.GetAll();

                if (activityTypes == null || activityTypes.Count() == 0)
                {
                    return BadRequest(new ErrorResponse(2, "No activity types found"));
                }

                return Ok(activityTypes);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse(1, "Any problem in the Server"));
            }
        }

        /// <summary>
        /// POST /activity-types
        /// Añade un nuevo tipo de actividad en 4VGym.
        /// </summary>
        /// <param name="activityType">ObjetoActivityType enviado en el cuerpo de la petición (JSON).</param>
        [HttpPost]
        [ProducesResponseType(typeof(ActivityType), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<ActivityType> CreateActivityType([FromBody] ActivityType activityType)
        {
            try
            {
                // 1. Validación básica de la entrada, objeto no nulo y obligatorio nombre de actividad
                // Esto se llaman condiciones de barrera!!!
                if (activityType == null)
                {
                    return BadRequest(new ErrorResponse(3, "Invalid activity type data"));
                }
                if (String.IsNullOrEmpty(activityType.Name))
                {
                    return BadRequest(new ErrorResponse(4, "The name of the activity is mandatory"));
                }

                // No puede haber tipos de actividad con el mismo nombre
                if (_repository.GetByName(activityType.Name) != null)
                {
                    return BadRequest(new ErrorResponse(5, "The name is unique, there is already another ActivityType with the same Name"));
                }


                // 2. Insertar/Guardar el objeto usando el repositorio
                ActivityType createdActivityType = _repository.Add(activityType);

                // 3. Retornar respuesta 201 Created con la ubicación o el objeto creado
                return StatusCode(StatusCodes.Status201Created, createdActivityType);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new ErrorResponse(1, "An internal server error occurred"));

            }
        }

    }

}
