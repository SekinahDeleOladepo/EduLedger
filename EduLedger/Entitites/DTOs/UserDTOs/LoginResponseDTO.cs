using EduLedger.Entitites.Models;

namespace EduLedger.Data.DTOs.UserDTOs
{
    public class LoginResponseDTO
    {
        public string UserName { get; set; }
        public UserProfile UserProfile { get; set; }
        //navigational properties

        public ICollection<Course> Courses { get; set; }

        public int ClassLevelId { get; set; }
        //navigational properties
        public ClassLevel ClassLevel { get; set; }
    }
}
