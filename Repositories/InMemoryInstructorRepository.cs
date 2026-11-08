using _4VGymAPI.Dtos;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace _4VGymAPI.Repositories
{
    public class InMemoryInstructorRepository
    {
        private Dictionary<long, InstructorCreateDto> _instructors = new();
        public InMemoryInstructorRepository()
        {
            SeedData(); // Precargamos algunos datos de prueba
        }
        private void SeedData()
        {

            _instructors = new Dictionary<long, InstructorCreateDto>
            {
                [1] = new InstructorCreateDto
                {
                    Id = 1,
                    Name = "Miguel Goyena",
                    Email = "miguel_goyena@cuatrovientos.org",
                    Phone = "654121212",
                    PhotoUrl = "https://img.magnific.com/vector-premium/imagen-perfil-avatar-hombre-aislada-fondo-imagen-profil-avatar-hombre_1293239-4841.jpg?semt=ais_hybrid&w=740&q=80"

                },
                [2] = new InstructorCreateDto
                {
                    Id = 2,
                    Name = "María Martín",
                    Email = "maria_martin@cuatrovientos.org",
                    Phone = "663423121",
                    PhotoUrl = "https://img.magnific.com/vector-premium/avatar-mujer-sonriente_937492-6135.jpg?semt=ais_hybrid&w=740&q=80"

                },
                [3] = new InstructorCreateDto
                {
                    Id = 3,
                    Name = "Iban Sarría",
                    Email = "iban_sarria@cuatrovientos.org",
                    Phone = "654646464",
                    PhotoUrl = "https://img.magnific.com/vector-premium/imagen-perfil-avatar-hombre-aislada-fondo-imagen-profil-avatar-hombre_1293239-4861.jpg?semt=ais_hybrid&w=740&q=80"
                }
            };


        }

        private List<InstructorCreateDto> ObtenerInstructoresDeBaseDeDatos()
        {
            // definir una lista de instructores para simular la obtención de datos desde una base de datos
            List<InstructorCreateDto> listaInstructores = new List<InstructorCreateDto>
            {
                new InstructorCreateDto { Id = 1, Name = "Miguel Goyena", Email = "miguel_goyena@cuatrovientos.org", Phone = "654121212", PhotoUrl = "https://img.magnific.com/vector-premium/imagen-perfil-avatar-hombre-aislada-fondo-imagen-profil-avatar-hombre_1293239-4841.jpg?semt=ais_hybrid&w=740&q=80" },
                new InstructorCreateDto { Id = 2, Name = "María Martín", Email = "maria_martin@cuatrovientos.org", Phone = "663423121", PhotoUrl = "https://img.magnific.com/vector-premium/avatar-mujer-sonriente_937492-6135.jpg?semt=ais_hybrid&w=740&q=80" },
                new InstructorCreateDto { Id = 3, Name = "Iban Sarría", Email = "iban_sarria@cuatrovientos.org", Phone = "654646464", PhotoUrl = "https://img.magnific.com/vector-premium/imagen-perfil-avatar-hombre-aislada-fondo-imagen-profil-avatar-hombre_1293239-4861.jpg?semt=ais_hybrid&w=740&q=80" }
            };

            return listaInstructores;
        }
        public IEnumerable<InstructorCreateDto> GetByFirstInitial(string? firstInitial = null)
        {
            IEnumerable<InstructorCreateDto> _allInstructors = _instructors.Values;

            // Si se recibe la inicial, aplicamos el filtro sobre la colección
            if (!string.IsNullOrWhiteSpace(firstInitial))
            {
                IEnumerable<InstructorCreateDto> filteredInstructors =
                    _allInstructors
                        .Where(instructor =>
                                !string.IsNullOrEmpty(instructor.Name) &&
                                instructor.Name.StartsWith(firstInitial, StringComparison.OrdinalIgnoreCase)
                        );

                return filteredInstructors;
            }

            return _allInstructors;
        }

        public IEnumerable<InstructorCreateDto> GetAll()
        {
            return _instructors.Values.ToList();
        }
        public InstructorCreateDto? GetById(int id)
        {
            _instructors.TryGetValue(id, out InstructorCreateDto? instructor);
            return instructor;
            // return _instructors.Values
            // .SingleOrDefault(i => i.Id == id); // FistOrDefault mejor que Where
        }
        public List<InstructorCreateDto> GetByIds(List<int> ids)
        {
            if (ids == null || !ids.Any())
                return new List<InstructorCreateDto>();

            return _instructors.Values
                .Where(instructor => ids.Contains(instructor.Id))
                .ToList();
        }
        public InstructorCreateDto? Add(InstructorCreateDto instructorNew, out ErrorResponse? errorResponse)
        {
            errorResponse = null;
            // Comprobación de duplicados (Email o Nombre existente, ignorando mayúsculas/minúsculas)
            bool exists = _instructors.Any(instructor =>
                instructor.Value.Email.Equals(instructorNew.Email, StringComparison.CurrentCultureIgnoreCase) ||
                instructor.Value.Name.Equals(instructorNew.Name, StringComparison.CurrentCultureIgnoreCase));

            if (exists)
            {
                errorResponse = new ErrorResponse(34, "An instructor with the same email or name already exists");
                return null;
            }
            int nuevoId = _instructors.Max(i => i.Value.Id) + 1;
            InstructorCreateDto instructor = new InstructorCreateDto(nuevoId, instructorNew.Name, instructorNew.Email, instructorNew.PhotoUrl, instructorNew.Phone);
            _instructors.Add(nuevoId, instructor);

            return instructor;
        }


    }
}
