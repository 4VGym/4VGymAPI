using _4VGymAPI.Data;
using _4VGymAPI.Dtos;
using _4VGymAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace _4VGymAPI.Repositories
{
    public class InstructorRepository
    {
        private readonly GymDbContext _gymDbContext;

        public InstructorRepository(GymDbContext gymDbContext)
        {
            _gymDbContext = gymDbContext;
        }

        public async Task<IEnumerable<InstructorCreateDto>> GetAll()
        {
            return await _gymDbContext.Instructors
                .Select(instructor => new InstructorCreateDto(instructor.Id, instructor.Name, instructor.Email, instructor.PhotoUrl, instructor.Phone))
                .ToListAsync();
        }

        public async Task<InstructorCreateDto?> GetById(int id)
        {
            Instructor? instructor = await _gymDbContext.Instructors.FindAsync(id);

            if (instructor == null)
            {
                return null;
            }

            return new InstructorCreateDto(instructor.Id, instructor.Name, instructor.Email, instructor.PhotoUrl, instructor.Phone);
        }
    }
}
