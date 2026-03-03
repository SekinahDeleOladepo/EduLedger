using EduLedger.Data.DTOs.ClassLevelDTOs;
using EduLedger.Entitites.DTOs.ClassLevelDTOs;
using EduLedger.Entitites.Models;

namespace EduLedger.Repository.IRepository
{
    public interface IClassLevelRepository
    {
        Task<BaseResponse> CreateClass(CreateClassDTO createClass);
        Task<BaseResponse> DeleteClass(int Id);
        Task<BaseResponse> UpdateClass(int id, UpdateClassLevelDTO updateClass);
        Task<BaseResponse> GetAllClass();
        Task<BaseResponse> GetClassById(int id);
        Task<BaseResponse> AssignStudentToClassLevel(int classLevelId, string userId);



    }
}
