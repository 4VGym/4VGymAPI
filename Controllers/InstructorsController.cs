using _4VGymAPI.Dtos;
using _4VGymAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;

namespace _4VGymAPI.Controllers
{
    [ApiController]
    [Route("instructors")]
    public class InstructorsController : ControllerBase
    {
        private readonly InstructorRepository _instructorRepository;

        public InstructorsController(InstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InstructorCreateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InstructorCreateDto>>> GetInstructors([FromQuery] int? id, [FromQuery(Name = "first_initial")] string? firstInitial)
        {
            try
            {
                if (id.HasValue)
                {
                    InstructorCreateDto? instructor = await _instructorRepository.GetById(id.Value);
                    if (instructor == null)
                    {
                        return NotFound(new ErrorResponse(2, $"No existe el instructor con ID {id.Value}"));
                    }
                    return Ok(new[] { instructor });
                }

                if (!string.IsNullOrWhiteSpace(firstInitial))
                {
                    IEnumerable<InstructorCreateDto> instructorsByInitial = await _instructorRepository.GetByFirstInitial(firstInitial);
                    return Ok(instructorsByInitial);
                }

                IEnumerable<InstructorCreateDto> allInstructors = await _instructorRepository.GetAll();
                return Ok(allInstructors);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ErrorResponse(1, $"Error interno: {ex.Message}")
                );
            }
        }
        private static readonly Regex SoloNumerosRegex = new Regex(@"^[0-9]+$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@gmail.com", RegexOptions.Compiled);

        [HttpPost]
        [ProducesResponseType(typeof(InstructorCreateDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorCreateDto>> CreateInstructor([FromBody] InstructorCreateDto instructorNew)
        {
            try
            {
                if (instructorNew.isEmpty())
                {
                    return BadRequest(new ErrorResponse(3, "Invalid instructor"));
                }

                if (string.IsNullOrWhiteSpace(instructorNew.Name))
                {
                    return BadRequest(new ErrorResponse(4, "The name of the instructor is mandatory"));
                }

                if (string.IsNullOrWhiteSpace(instructorNew.Phone) || !SoloNumerosRegex.IsMatch(instructorNew.Phone))
                {
                    return BadRequest(new ErrorResponse(4, "The telephone of the instructor is mandatory and must contain only digits"));
                }

                if (string.IsNullOrWhiteSpace(instructorNew.Email) || !EmailRegex.IsMatch(instructorNew.Email))
                {
                    return BadRequest(new ErrorResponse(4, "The email is mandatory and must be a valid email address"));
                }

                if (string.IsNullOrWhiteSpace(instructorNew.PhotoUrl) ||
                    !Uri.TryCreate(instructorNew.PhotoUrl, UriKind.Absolute, out Uri? uriResult) ||
                    (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                {
                    return BadRequest(new ErrorResponse(4, "The photo URL is invalid. It must start with http:// or https://"));
                }


                InstructorCreateDto? instructor = await _instructorRepository.Add(instructorNew);
                if (instructor == null)
                {
                    return BadRequest(new ErrorResponse(34, "An instructor with the same email or name already exists"));
                }
                return StatusCode(StatusCodes.Status201Created, instructor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new ErrorResponse(1, "An internal server error occurred"));
            }
        }

    }
}
