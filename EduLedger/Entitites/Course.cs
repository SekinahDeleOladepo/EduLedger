namespace EduLedger.Data
{
    public class Course: Base
    {
        public string Name { get; set; }

        public int? ClassLevelId { get; set; }
        public ClassLevel ClassLevel { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
