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
    }
}
