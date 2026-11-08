using _4VGymAPI.Dtos;

namespace _4VGymAPI.Repositories
{
    public class InMemoryActivityTypeRepository
    {
        private List<ActivityTypeCreateDto> _activityTypes = new();

        private int _nextId = 1; // Para el autonumerico
        public InMemoryActivityTypeRepository()
        {
            SeedData(); // Precargamos algunos datos de prueba
        }
        private void SeedData()
        {
            _activityTypes = new List<ActivityTypeCreateDto>()
            {
                new ActivityTypeCreateDto (_nextId++, "BodyPump",  2 ),
                new ActivityTypeCreateDto ( _nextId++, "Spinning",  1 ),
                new ActivityTypeCreateDto { Id = _nextId++, Name = "Pilates", NumberMonitors = 1 }
            };
            _activityTypes.Add(new ActivityTypeCreateDto(40, "CrossFit", 3));
        }
        // Método para obtener todos los tipos de actividades
        public IEnumerable<ActivityTypeCreateDto> GetAll()
        {
            return _activityTypes;
        }



        /// <summary>
        /// Guarda un nuevo tipo de actividad generando su ID de forma automática. (Autonumerico)
        /// </summary>
        /// <param name="activityType">Objeto con los datos a guardar (sin ID o con ID no asignado).</param>
        /// <returns>La entidad guardada con su ID autogenerado.</returns>
        public ActivityTypeCreateDto Add(ActivityTypeCreateDto activityType)
        {
            // Asignamos el ID autoincremental en la capa de datos, o sea que machacamos el que venia
            activityType.Id = _nextId++;

            _activityTypes.Add(activityType);

            // Devolvemos el objeto completo tal como se ha guardado
            return activityType;
        }

        // Método para obtener todos los tipos de actividades por Nombre
        public ActivityTypeCreateDto? GetByName(string name)
        {
            return _activityTypes.SingleOrDefault(at =>
                            string.Equals(at.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public ActivityTypeCreateDto? GetById(int? id)
        {
            if (id == null)
                return null;

            return _activityTypes
                .SingleOrDefault(i => i.Id == id); // FistOrDefault mejor que Where
        }


    }

}
