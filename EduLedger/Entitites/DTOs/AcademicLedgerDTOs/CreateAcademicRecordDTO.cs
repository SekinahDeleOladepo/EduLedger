using System.ComponentModel.DataAnnotations;

namespace EduLedger.Entitites.DTOs.AcademicLedgerDTOs
{
    public class CreateAcademicRecordDTO
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Session { get; set; }   

        [Required]
        [MaxLength(20)]
        public string Term { get; set; }      

        [Range(0, 40)]
        public double CA1 { get; set; }

        [Range(0, 40)]
        public double CA2 { get; set; }

        [Range(0, 100)]
        public double Exam { get; set; }
    }
}

