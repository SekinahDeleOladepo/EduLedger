using EduLedger.Data;
using EduLedger.Data.DTOs.ClassLevelDTOs;
using EduLedger.Entitites.DTOs.ClassLevelDTOs;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduLedger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassLevelsController : ControllerBase
    {
        private readonly EduLedgerDBContext _context;

        public ClassLevelsController(EduLedgerDBContext context)
        {
            _context = context;
        }

        // GET: api/ClassLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassLevel>>> GetClassLevels()
        {
            var classLevels = await _context.Courses.Where(x => x.IsActive).ToListAsync();
            return classLevels;
        }

        // GET: api/ClassLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassLevel>> GetClassLevel(int id)
        {
            var classLevel = await _context.Courses.Where(x => x.IsActive).FirstOrDefaultAsync(u => u.Id == id);

            if (classLevel == null)
            {
                return NotFound();
            }

            return classLevel;
        }

        // PUT: api/ClassLevels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassLevel(int id,UpdateClassLevelDTO updateClass)
        {
            var classLevel = await _context.Courses.FindAsync(id);

            if (classLevel == null || !classLevel.IsActive)
                return NotFound();

            classLevel.Name = updateClass.Name;
            classLevel.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _context.Entry(classLevel).State = EntityState.Modified;


            return NoContent();
            
        }

        // POST: api/ClassLevels
        [HttpPost]
        public async Task<ActionResult<ClassLevel>> CreateClassLevel(CreateClassDTO createClass)
        {
            var exists = await _context.Courses
            .AnyAsync(x => x.Name == createClass.Name && x.IsActive);

            if (exists)
                return BadRequest("Class level already exists");

            var classLevel = new ClassLevel
            {
                Name = createClass.Name
            };
            _context.Courses.Add(classLevel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClassLevel", new { id = classLevel.Id }, classLevel);
        }

        // DELETE: api/ClassLevels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassLevel(int id)
        {
            var classLevel = await _context.Courses.FindAsync(id);
            if (classLevel == null || !classLevel.IsActive)
            {
                return NotFound();
            }

           classLevel.IsActive = false;
            classLevel.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("{classLevelId}/assign-student/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignStudentToClassLevel(
    int classLevelId,
    string userId)
        {
            var classLevel = await _context.Courses
                            .FirstOrDefaultAsync(x => x.Id == classLevelId && x.IsActive);
            if (classLevel == null)
                return NotFound("Class level not found");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");
            user.ClassLevelId = classLevelId;
            await _context.SaveChangesAsync();

            return Ok("Student assigned successfully");
        }
    }
}
