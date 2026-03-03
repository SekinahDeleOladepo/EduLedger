using EduLedger.Data.DTOs.CourseDTOs;
using EduLedger.Entitites.DTOs.CourseDTOs;
using EduLedger.Entitites.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduLedger.Repository.IRepository
{
    public interface ICoursesRepository
    {
        Task<BaseResponse> CreateCourse(CreateCourseDTO createCourse);
        public  Task<BaseResponse> GetCoursesByFilter([FromQuery] int? classLevelId,
[FromQuery] string? instructorId);
        public  Task<BaseResponse> GetCourse(int id);
        Task<BaseResponse> DeleteCourse(DeleteCourseDTO deleteCourseDTO);
        Task<BaseResponse> UpdateCourse(int id, UpdateCourseDTO updateCourse);
        Task<BaseResponse> AssignCourseToClassLevel(int courseId, int classLevelId);

    }
}
