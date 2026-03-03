namespace EduLedger.Data.DTOs.CourseDTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ClassLevelId { get; set; }
        public string? InstructorId { get; set; }
    }
}
