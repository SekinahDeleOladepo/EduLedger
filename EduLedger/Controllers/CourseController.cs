using EduLedger.Data;
using EduLedger.Data.DTOs.ClassLevelDTOs;
using EduLedger.Data.DTOs.CourseDTOs;
using EduLedger.Entitites.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduLedger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
  
        private readonly EduLedgerDBContext _context;

        public CourseController(EduLedgerDBContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var courses = await _context.Courses.Where(x => x.IsActive).ToListAsync();
            return courses;
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.Where(x => x.IsActive).FirstOrDefaultAsync(u => u.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/ClassLevels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDTO updateCourse)
        {
            var course = await _context.Courses
                .FindAsync(id);

            if (course == null || !course.IsActive)
                return NotFound();

            course.Name = updateCourse.Name;
            course.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _context.Entry(course).State = EntityState.Modified;


            return NoContent();

        }

        // POST: api/ClassLevels
        [HttpPost]
        public async Task<ActionResult<ClassLevel>> CreateCourse(CreateCourseDTO createCourse)
        {
            var exists = await _context.Courses
            .AnyAsync(x => x.Name == createCourse.Name && x.IsActive);

            if (exists)
                return BadRequest("Course already exists");

            var course = new Course
            {
                Name = createCourse.Name
            };
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null || !course.IsActive)
            {
                return NotFound();
            }

            course.IsActive = false;
            course.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("{courseId}/assign-student/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignStudentToCourse(
    int courseId,
    string userId)
        {
            var course = await _context.Courses
                           .Include(c => c.Users)
        .FirstOrDefaultAsync(c => c.Id == courseId && c.IsActive);

            if (course == null)
                return NotFound("Course not found");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");
            if (course.Users.Any(u => u.Id == userId))
                return BadRequest("Student already enrolled in this course");
            course.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok("Course assigned to Student  successfully");
        }

        public async Task<IActionResult> AssignCourseToClassLevel(int courseId, int classLevelId)
        {
            // Get course
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId && c.IsActive);

            if (course == null)
                return NotFound("Course not found");

            // Get class level including its courses
            var classLevel = await _context.ClassLevels
                .Include(cl => cl.Courses)
                .FirstOrDefaultAsync(cl => cl.Id == classLevelId);

            if (classLevel == null)
                return NotFound("Class level not found");

            // Check if course is already assigned
            if (classLevel.Courses.Any(c => c.Id == courseId))
                return BadRequest("Course already assigned to this class level");

            // Assign course
            classLevel.Courses.Add(course);

            await _context.SaveChangesAsync();

            return Ok("Course assigned to class level successfully");
        }

    }

}

