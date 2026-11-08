using _4VGymAPI.Data;
using _4VGymAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace _4VGymAPI.Repositories
{
    public class ActivityTypeRepository
    {
        private readonly GymDbContext _gymDbContext;

        public ActivityTypeRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }

        public async Task<IEnumerable<ActivityTypeCreateDto>> GetAll()
        {
            return await _gymDbContext.ActivityTypes
                .Select(activityType => new ActivityTypeCreateDto(activityType.Id, activityType.Name, activityType.NumberMonitors))
                .ToListAsync();
        }

        public async Task<ActivityTypeCreateDto?> GetByName(string name)
        {
            return await _gymDbContext.ActivityTypes
                .Where(activityType => activityType.Name.ToUpper() == name.ToUpper())
                .Select(activityType => new ActivityTypeCreateDto(activityType.Id, activityType.Name, activityType.NumberMonitors))
                .SingleOrDefaultAsync();
        }
    }
}
