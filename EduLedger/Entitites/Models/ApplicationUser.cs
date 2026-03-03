using Microsoft.AspNetCore.Identity;

namespace EduLedger.Entitites.Models
{
    public class ApplicationUser: IdentityUser
    {
        //navigational properties
        public  UserProfile UserProfile { get; set; } = null!;
        //navigational properties
        // Student
        public ICollection<Course> EnrolledCourses { get; set; }

        // Instructor
        public ICollection<Course> CoursesTaught { get; set; }

        public ICollection<AcademicRecord> AcademicRecords { get; set; }

        public int? ClassLevelId { get; set; }
        //navigational properties
        public ClassLevel ClassLevel { get; set; } = null!;
    }
}
