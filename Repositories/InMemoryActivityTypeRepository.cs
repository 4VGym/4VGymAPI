using _4VGymAPI.Models;

namespace _4VGymAPI.Repositories
{
    public class InMemoryActivityTypeRepository
    {
        private List<ActivityType> _activityTypes = new();

        private int _nextId = 1; // Para el autonumerico
        public InMemoryActivityTypeRepository()
        {
            SeedData(); // Precargamos algunos datos de prueba
        }
        private void SeedData()
        {
            _activityTypes = new List<ActivityType>()
            {
                new ActivityType (_nextId++, "BodyPump",  2 ),
                new ActivityType ( _nextId++, "Spinning",  1 ),
                new ActivityType { Id = _nextId++, Name = "Pilates", NumberMonitors = 1 }
            };
            _activityTypes.Add(new ActivityType(40, "CrossFit", 3));
        }
        // Método para obtener todos los tipos de actividades
        public IEnumerable<ActivityType> GetAll()
        {
            return _activityTypes;
        }



        /// <summary>
        /// Guarda un nuevo tipo de actividad generando su ID de forma automática. (Autonumerico)
        /// </summary>
        /// <param name="activityType">Objeto con los datos a guardar (sin ID o con ID no asignado).</param>
        /// <returns>La entidad guardada con su ID autogenerado.</returns>
        public ActivityType Add(ActivityType activityType)
        {
            // Asignamos el ID autoincremental en la capa de datos, o sea que machacamos el que venia
            activityType.Id = _nextId++;

            _activityTypes.Add(activityType);

            // Devolvemos el objeto completo tal como se ha guardado
            return activityType;
        }

        // Método para obtener todos los tipos de actividades por Nombre
        public ActivityType? GetByName(string name)
        {
            return _activityTypes.SingleOrDefault(at =>
                            string.Equals(at.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public ActivityType? GetById(int? id)
        {
            if (id == null)
                return null;

            return _activityTypes
                .SingleOrDefault(i => i.Id == id); // FistOrDefault mejor que Where
        }


    }

}
