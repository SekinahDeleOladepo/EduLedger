using EduLedger.Data;

namespace EduLedger.Entitites.Models
{
    public class Course: Base
    {
        public string Name { get; set; }

        public int? ClassLevelId { get; set; }
        public string? InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; }
        public ClassLevel ClassLevel { get; set; }

        public ICollection<ApplicationUser> Students { get; set; }
        public ICollection<AcademicRecord> AcademicRecords { get; set; }


    }
}
