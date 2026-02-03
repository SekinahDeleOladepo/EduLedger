using Microsoft.AspNetCore.Identity;

namespace EduLedger.Data
{
    public class ApplicationUser: IdentityUser
    {
        //navigational properties
        public UserProfile UserProfile { get; set; }
        //navigational properties

        public ICollection<Course> Courses { get; set; }

        public int? ClassLevelId { get; set; }
        //navigational properties
        public ClassLevel ClassLevel { get; set; }
    }
}
