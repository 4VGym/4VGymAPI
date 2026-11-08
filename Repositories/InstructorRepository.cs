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

        public async Task<IEnumerable<InstructorCreateDto>> GetByFirstInitial(string firstInitial)
        {
            return await _gymDbContext.Instructors
                .Where(instructor => instructor.Name.ToUpper().StartsWith(firstInitial.ToUpper()))
                .Select(instructor => new InstructorCreateDto(instructor.Id, instructor.Name, instructor.Email, instructor.PhotoUrl, instructor.Phone))
                .ToListAsync();
        }

        public async Task<InstructorCreateDto?> Add(InstructorCreateDto instructorNew)
        {
            bool exists = await _gymDbContext.Instructors.AnyAsync(instructor =>
                instructor.Email.ToUpper() == instructorNew.Email.ToUpper() ||
                instructor.Name.ToUpper() == instructorNew.Name.ToUpper());

            if (exists)
            {
                return null;
            }

            Instructor instructor = new Instructor
            {
                Name = instructorNew.Name,
                Email = instructorNew.Email,
                PhotoUrl = instructorNew.PhotoUrl,
                Phone = instructorNew.Phone
            };

            _gymDbContext.Instructors.Add(instructor);
            await _gymDbContext.SaveChangesAsync();

            return new InstructorCreateDto(instructor.Id, instructor.Name, instructor.Email, instructor.PhotoUrl, instructor.Phone);
        }
    }
}
