using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using _4VGymAPI.Validations;

namespace _4VGymAPI.Models
{
    public class Activity
    {
        public int Id { get; set; }

        public int? ActivityTypeId { get; set; }

        public int MaxParticipants { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; } 


        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

        public ActivityType? ActivityType { get; set; }


    }

}
