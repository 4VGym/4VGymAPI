using _4VGymAPI.Dtos;
using _4VGymAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _4VGymAPI.Controllers
{
    [ApiController]
    [Route("activities")]
    public class ActivityController : ControllerBase
    {

        private readonly ActivityRepository _activityRepository;

        public ActivityController(ActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ActivityCreateDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ActivityCreateDto>> CreateActivity([FromBody] ActivityCreateDto activity)
        {
            try
            {
                List<ErrorResponse> errors = new List<ErrorResponse>();
                ActivityCreateDto? activityCreated = await _activityRepository.Add(activity, errors);
                if (activityCreated == null)
                {
                    return BadRequest(errors);
                }

                return StatusCode(StatusCodes.Status201Created, activity);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new ErrorResponse(1, "An internal server error occurred"));

            }
        }
    }

}
