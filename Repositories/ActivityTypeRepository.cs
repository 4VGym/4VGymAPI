using _4VGymAPI.Data;
using _4VGymAPI.Dtos;
using _4VGymAPI.Models;
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

        public async Task<ActivityTypeCreateDto> Add(ActivityTypeCreateDto activityType)
        {
            ActivityType newActivityType = new ActivityType
            {
                Name = activityType.Name,
                NumberMonitors = activityType.NumberMonitors
            };

            _gymDbContext.ActivityTypes.Add(newActivityType);
            await _gymDbContext.SaveChangesAsync();

            activityType.Id = newActivityType.Id;
            return activityType;
        }

        public async Task<ActivityTypeCreateDto?> GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            ActivityType? activityType = await _gymDbContext.ActivityTypes.FindAsync(id);

            if (activityType == null)
            {
                return null;
            }

            return new ActivityTypeCreateDto(activityType.Id, activityType.Name, activityType.NumberMonitors);
        }
    }
}
