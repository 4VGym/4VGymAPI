using _4VGymAPI.Data;
using _4VGymAPI.Dtos;
using _4VGymAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace _4VGymAPI.Repositories
{
    public class ActivityRepository
    {
        private readonly GymDbContext _gymDbContext;
        private readonly ActivityTypeRepository _activityTypeRepository;
        private readonly InstructorRepository _instructorRepository;

        public ActivityRepository(GymDbContext gymDbContext, ActivityTypeRepository activityTypeRepository, InstructorRepository instructorRepository)
        {
            _gymDbContext = gymDbContext;
            _activityTypeRepository = activityTypeRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<ActivityCreateDto?> Add(ActivityCreateDto activity, List<ErrorResponse> errorResponse)
        {
            if (await _activityTypeRepository.GetById(activity.ActivityTypeId) == null)
            {
                errorResponse.Add(new ErrorResponse(34, "EL tipo de actividad debe de funcionar"));
            }

            foreach (int instructorId in activity.InstructorsId)
            {
                if (await _instructorRepository.GetById(instructorId) == null)
                {
                    errorResponse.Add(new ErrorResponse(35, "El instructor: " + instructorId + " no existe"));
                }
            }

            if (errorResponse.Count > 0)
            {
                return null;
            }

            List<Instructor> instructors = await _gymDbContext.Instructors
                .Where(instructor => activity.InstructorsId.Contains(instructor.Id))
                .ToListAsync();

            Activity newActivity = new Activity
            {
                ActivityTypeId = activity.ActivityTypeId,
                MaxParticipants = activity.MaxParticipants,
                DateStart = activity.DateStart,
                DateEnd = activity.DateEnd,
                Instructors = instructors
            };

            _gymDbContext.Activities.Add(newActivity);
            await _gymDbContext.SaveChangesAsync();

            activity.Id = newActivity.Id;
            return activity;
        }
    }
}
