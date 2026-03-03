using EduLedger.Data;
using EduLedger.Entitites;
using EduLedger.Entitites.DTOs.AcademicLedgerDTOs;
using EduLedger.Entitites.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduLedger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicLedgerController : ControllerBase
    {
        private readonly EduLedgerDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AcademicLedgerController(
            EduLedgerDBContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost("record")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> RecordScore(CreateAcademicRecordDTO dto)
        {
            var exists = await _context.AcademicRecords.AnyAsync(x =>
                x.StudentId == dto.StudentId &&
                x.CourseId == dto.CourseId &&
                x.Session == dto.Session &&
                x.Term == dto.Term);

            if (exists)
                return BadRequest("Academic record already exists for this term");

            var total = dto.CA1 + dto.CA2 + dto.Exam;

            var grade = CalculateGrade(total);

            var record = new AcademicRecord
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Session = dto.Session,
                Term = dto.Term,
                CA1 = dto.CA1,
                CA2 = dto.CA2,
                Exam = dto.Exam,
                TotalScore = total,
                Grade = grade
            };

            _context.AcademicRecords.Add(record);
            await _context.SaveChangesAsync();

            return Ok("Academic record created");
        }
        private string CalculateGrade(double total)
        {
            if (total >= 70) return "A";
            if (total >= 60) return "B";
            if (total >= 50) return "C";
            if (total >= 45) return "D";
            if (total >= 40) return "E";
            return "F";
        }
        [HttpGet("student/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetStudentAcademicLedger(string studentId)
        {
            var records = await _context.AcademicRecords
                .Include(r => r.Course)
                .Where(r => r.StudentId == studentId && r.IsActive)
                .ToListAsync();

            if (!records.Any())
                return NotFound("No academic records found");

            var average = records.Average(r => r.TotalScore);

            var result = new
            {
                StudentId = studentId,
                OverallAverage = average,
                Records = records.Select(r => new
                {
                    r.Course.Name,
                    r.Session,
                    r.Term,
                    r.TotalScore,
                    r.Grade
                })
            };

            return Ok(result);
        }


    }
}
