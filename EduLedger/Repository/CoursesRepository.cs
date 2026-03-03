using EduLedger.Data;
using EduLedger.Data.DTOs.CourseDTOs;
using EduLedger.Entitites.DTOs.CourseDTOs;
using EduLedger.Entitites.Models;
using EduLedger.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduLedger.Repository
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly EduLedgerDBContext _context;
        public CoursesRepository(EduLedgerDBContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse> CreateCourse(CreateCourseDTO createCourse)
        {
            var exists = await _context.Courses
             .AnyAsync(x => x.Name == createCourse.Name && x.IsActive);

                if (!exists)
                {
                    var newCourse = new Course
                    {
                        Name = createCourse.Name,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true

                    };
                    await _context.Courses.AddAsync(newCourse);
                    await _context.SaveChangesAsync();
                    var result = new
                    {
                        Id = newCourse.Id,
                        Name = newCourse.Name.ToUpper()
                    };
                    return new BaseResponse
                    {
                        Status = true,
                        Message = "Course created successfully",
                        Data = result
                    };
                }
            return new BaseResponse
            {
                Status = false,
                Message = "Course already exists",
                Data = null
            };

        }

        public async Task<BaseResponse> DeleteCourse(DeleteCourseDTO deleteCourseDTO)
        {
            var course = await _context.Courses.FindAsync(deleteCourseDTO);
            if (course == null || !course.IsActive)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Name does not exists",
                    Data = null
                };
            }

            course.IsActive = false;
            course.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                Status = true,
                Message = "Class deleted successfuly",
                Data = null
            };
        }
        public async Task<BaseResponse>GetCoursesByFilter([FromQuery] int? classLevelId,
[FromQuery] string? instructorId)
        {

            var query = _context.Courses
               .Where(c => c.IsActive)
               .AsQueryable();

            if (classLevelId.HasValue)
                query = query.Where(c => c.ClassLevelId == classLevelId);

            if (!string.IsNullOrEmpty(instructorId))
                query = query.Where(c => c.InstructorId == instructorId);

            var courses = await query
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Name = c.Name.ToUpper(),
                    ClassLevelId = c.ClassLevelId,
                    InstructorId = c.InstructorId
                })
                .ToListAsync();

            return new BaseResponse
            {
                Status = true,
                Message = "Here ",
                Data = courses
            };
        }

        public async Task<BaseResponse> GetCourse(int id)
        {
            var course = await _context.Courses.Where(x => x.IsActive).FirstOrDefaultAsync(u => u.Id == id);

            if (course == null)
                return new BaseResponse
                {
                    Status = false,
                    Message = "Course not found",
                    Data = null
                };

            return new BaseResponse
            {
                Status = true,
                Message = "Here is the course with this Id ",
                Data = course
            };
        }

        public async Task<BaseResponse> UpdateCourse(int id, UpdateCourseDTO updateCourse)
        {
            var course = await _context.Courses
                .FindAsync(id);

            if (course == null || !course.IsActive)
                return new BaseResponse
                {
                    Status = false,
                    Message = "Course not found",
                    Data = null
                };

            course.Name = updateCourse.Name;
            course.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
           
            return new BaseResponse
            {
                Status = true,
                Message = "Class deleted successfuly",
                Data = null
            };
        }
        public async Task<BaseResponse> AssignCourseToClassLevel(int courseId, int classLevelId)
        {
            // Get course
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId && c.IsActive);

            if (course == null)
            return new BaseResponse
            {
                Status = false,
                Message = "Course not found",
                Data = null
            };
            // Get class level including its courses
            var classLevel = await _context.ClassLevels
                .Include(cl => cl.Courses)
                .FirstOrDefaultAsync(cl => cl.Id == classLevelId);

            if (classLevel == null)
            return new BaseResponse
            {
                Status = false,
                Message = "Class level not found",
                Data = null
            };
            // Check if course is already assigned
            if (classLevel.Courses.Any(c => c.Id == courseId))
            return new BaseResponse
            {
                Status = false,
                Message = "Course already assigned to this class level",
                Data = null
            };
            // Assign course
            course.ClassLevelId = classLevelId;

            //classLevel.Courses.Add(course);

            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                Status = true,
                Message = "Course assigned to class level successfully",
                Data = null
            };
        }
    }
}
