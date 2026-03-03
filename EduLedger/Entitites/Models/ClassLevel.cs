using EduLedger.Data;

namespace EduLedger.Entitites.Models
{
    public class ClassLevel:Base
    {
        public string Name { get; set; } = null!;
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
