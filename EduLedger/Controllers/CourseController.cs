using EduLedger.Data;
using EduLedger.Data.DTOs.ClassLevelDTOs;
using EduLedger.Data.DTOs.CourseDTOs;
using EduLedger.Entitites.DTOs.CourseDTOs;
using EduLedger.Entitites.Models;
using EduLedger.Repository.IRepository;
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
        private readonly ICoursesRepository _courseRepository;

        public CourseController(EduLedgerDBContext context, ICoursesRepository courseRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
        }

        // GET: api/Courses
        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCoursesByFilter([FromQuery] int? classLevelId,
    [FromQuery] string? instructorId )
        {
            var response = await _courseRepository.GetCoursesByFilter(classLevelId, instructorId);
            if (!response.Status) return BadRequest(response);
            return Ok(response);

        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var response = await _courseRepository.GetCourse(id);
            if (!response.Status) return BadRequest(response);
            return Ok(response);
        }

        // PUT: api/ClassLevels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, UpdateCourseDTO updateCourse)
        {
            var response = await _courseRepository.UpdateCourse(id,updateCourse);
            if (!response.Status) return BadRequest(response);
            return Ok(response);


        }

        // POST: api/ClassLevels
        [HttpPost]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCourse(CreateCourseDTO createCourse)
        {
            var response = await _courseRepository.CreateCourse(createCourse);
            if (!response.Status) return BadRequest(response);
            return Ok(response);

            
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(DeleteCourseDTO deleteCourseDTO)
        {
            var response = await _courseRepository.DeleteCourse(deleteCourseDTO);
            if (!response.Status) return BadRequest(response);
            return Ok(response);
        }
       
        [HttpPost("{courseId}/assign-Course/{classLevelId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignCourseToClassLevel(int courseId, int classLevelId)
        {
            var response = await _courseRepository.AssignCourseToClassLevel(courseId, classLevelId);
            if (!response.Status) return BadRequest(response);
            return Ok(response);
        }

    }

}

