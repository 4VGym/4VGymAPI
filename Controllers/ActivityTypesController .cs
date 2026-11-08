using _4VGymAPI.Dtos;
using _4VGymAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _4VGymAPI.Controllers
{
    [ApiController]
    [Route("activity-types")]
    public class ActivityTypesController : ControllerBase
    {
        private readonly ActivityTypeRepository _activityTypeRepository;

        public ActivityTypesController(ActivityTypeRepository activityTypeRepository)
        {
            _activityTypeRepository = activityTypeRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ActivityTypeCreateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ActivityTypeCreateDto>>> FindActivityTypes()
        {
            try
            {
                IEnumerable<ActivityTypeCreateDto> activityTypes = await _activityTypeRepository.GetAll();

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

        [HttpPost]
        [ProducesResponseType(typeof(ActivityTypeCreateDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ActivityTypeCreateDto>> CreateActivityType([FromBody] ActivityTypeCreateDto activityType)
        {
            try
            {
                if (activityType == null)
                {
                    return BadRequest(new ErrorResponse(3, "Invalid activity type data"));
                }
                if (String.IsNullOrEmpty(activityType.Name))
                {
                    return BadRequest(new ErrorResponse(4, "The name of the activity is mandatory"));
                }

                if (await _activityTypeRepository.GetByName(activityType.Name) != null)
                {
                    return BadRequest(new ErrorResponse(5, "The name is unique, there is already another ActivityType with the same Name"));
                }


                ActivityTypeCreateDto createdActivityType = await _activityTypeRepository.Add(activityType);

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
