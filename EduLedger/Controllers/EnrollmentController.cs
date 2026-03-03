using EduLedger.Data;
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
    public class EnrollmentController : ControllerBase
    {
        private readonly EduLedgerDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnrollmentController(
            EduLedgerDBContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //[HttpPost("{courseId}/assign-student/{userId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AssignStudentToCourse(int courseId, string userId)
        //{
        //    var course = await _context.Courses
        //                   .Include(c => c.Students)
        //.FirstOrDefaultAsync(c => c.Id == courseId && c.IsActive);

        //    if (course == null)
        //        return NotFound("Course not found");

        //    var Student = await _context.Users.FindAsync(userId);
        //    if (Student == null)
        //        return NotFound("User not found");
        //    if (course.Students.Any(u => u.Id == userId))
        //        return BadRequest("Student already enrolled in this course");
        //    course.Students.Add(Student);

        //    await _context.SaveChangesAsync();

        //    return Ok("Course assigned to Student  successfully");
        //}
        [HttpPost("enroll")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> EnrollStudent(int courseId, string studentId)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId && c.IsActive);

            if (course == null)
                return NotFound("Course not found");

            var student = await _context.Users
                .Include(u => u.EnrolledCourses)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            if (student == null)
                return NotFound("Student not found");

            // 🔒 Rule 1: Must be student role
            if (!await _userManager.IsInRoleAsync(student, "Student"))
                return BadRequest("User is not a student");

            // 🔒 Rule 2: Prevent duplicate enrollment
            if (course.Students.Any(s => s.Id == studentId))
                return BadRequest("Student already enrolled");

            // 🔒 Rule 3: ClassLevel must match
            if (course.ClassLevelId != student.ClassLevelId)
                return BadRequest("Student not in this class level");

            course.Students.Add(student);

            await _context.SaveChangesAsync();

            return Ok("Student enrolled successfully");
        }
        [HttpDelete("unenroll")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UnenrollStudent(int courseId, string studentId)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return NotFound("Course not found");

            var student = course.Students
                .FirstOrDefault(s => s.Id == studentId);

            if (student == null)
                return BadRequest("Student not enrolled in this course");

            course.Students.Remove(student);

            await _context.SaveChangesAsync();

            return Ok("Student unenrolled successfully");
        }
        [HttpGet("student/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetStudentCourses(string studentId)
        {
            var student = await _context.Users
                .Include(u => u.EnrolledCourses)
                .ThenInclude(c => c.ClassLevel)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            if (student == null)
                return NotFound("Student not found");

            var result = student.EnrolledCourses
                .Where(c => c.IsActive)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    ClassLevel = c.ClassLevel.Name
                });

            return Ok(result);
        }
        [HttpGet("courses/{courseId}/students")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> GetStudentsInCourse(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId && c.IsActive);

            if (course == null)
                return NotFound("Course not found");

            var students = course.Students.Select(s => new
            {
                s.Id,
                s.Email
            });

            return Ok(students);
        }


    }

}
