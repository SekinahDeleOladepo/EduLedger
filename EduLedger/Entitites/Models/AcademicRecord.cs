using EduLedger.Data;

namespace EduLedger.Entitites.Models
{
    public class AcademicRecord : Base
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string Session { get; set; }   
        public string Term { get; set; }     

        public double CA1 { get; set; }
        public double CA2 { get; set; }
        public double Exam { get; set; }

        public double TotalScore { get; set; }
        public string Grade { get; set; }
    }

}
