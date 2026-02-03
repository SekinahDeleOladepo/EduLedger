namespace EduLedger.Data
{
    public class ClassLevel:Base
    {
        public string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
