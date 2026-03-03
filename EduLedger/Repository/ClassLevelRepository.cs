using EduLedger.Data;
using EduLedger.Data.DTOs.ClassLevelDTOs;
using EduLedger.Entitites.DTOs.ClassLevelDTOs;
using EduLedger.Entitites.Models;
using EduLedger.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduLedger.Repository
{
    public class ClassLevelRepository : IClassLevelRepository
    {
        private readonly EduLedgerDBContext _context;
        public ClassLevelRepository(EduLedgerDBContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse> GetAllClass()
        {
            var classList = await _context.ClassLevels
                .Where(x => x.IsActive)
                .Select(x => new 
                {
                    Id = x.Id,
                    Name = x.Name.ToUpper(),

                })
                .ToListAsync();
            if (classList.Count == 0)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "No class found",
                    Data = null

                };
            }
            return new BaseResponse
            {
                Status = true,
                Message = "Successful",
                Data = classList

            };
        
        }
        public async Task<BaseResponse> GetClassById(int id)
        {
            var classLevel = await _context.ClassLevels.Where(x => x.IsActive).FirstOrDefaultAsync(u => u.Id == id);

            if (classLevel == null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "",
                    Data = null
                };
            }
            return new BaseResponse
            {
                Status = true,
                Message = "Class updated successfully",
                Data = new
                {
                    Id = classLevel.Id,
                    Name = classLevel.Name
                }
            };
        }


        public async Task<BaseResponse> CreateClass(CreateClassDTO createClass)
        {
            var exists = await _context.ClassLevels
          .AnyAsync(x => x.Name.ToUpper() == createClass.Name.ToUpper() && x.IsActive);
            if (!exists)
            {
                var newClass = new ClassLevel
                {
                    Name = createClass.Name,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true

                };
                await _context.ClassLevels.AddAsync(newClass);
                await _context.SaveChangesAsync();
                var result = new
                {
                    Id = newClass.Id,
                    Name = newClass.Name.ToUpper()
                };
                return new BaseResponse
                {
                    Status = true,
                    Message = "Class created successfully",
                    Data = result
                };
            }
            return new BaseResponse
            {
                Status = false,
                Message = "Name already exists",
                Data = null
            };



        }

        public async Task<BaseResponse> DeleteClass(int Id)
        {
            var classLevel = await _context.ClassLevels.FindAsync(Id);
            if (classLevel == null || !classLevel.IsActive)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Name does not exists",
                    Data = null
                };
            }
            classLevel.IsActive = false;
            classLevel.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                Status = true,
                Message = "Class deleted successfuly",
                Data = null
            };
        }
      
        public async Task<BaseResponse> UpdateClass(int id, UpdateClassLevelDTO updateClass)
        {
            var classLevel = await _context.ClassLevels.FindAsync(id);

            if (classLevel == null || !classLevel.IsActive)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Class does not exist",
                    Data = null
                };
            }
            var exists = await _context.ClassLevels
        .AnyAsync(x => x.Name == updateClass.Name
                       && x.Id != id
                       && x.IsActive);
            if (exists)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Name already exists",
                    Data = null
                };
            }


            classLevel.Name = updateClass.Name;
            classLevel.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                Status = true,
                Message = "Class updated successfully",
                Data = new
                {
                    Id = classLevel.Id,
                    Name = classLevel.Name
                }
            };

        }
        public async Task<BaseResponse> AssignStudentToClassLevel(
    int classLevelId,
    string userId)
        {
            var classLevel = await _context.ClassLevels
                            .FirstOrDefaultAsync(x => x.Id == classLevelId && x.IsActive);
            if (classLevel == null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Class level not found",
                    Data = null
                };
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "User not found",
                    Data = null
                };
            }
            if (user.ClassLevelId == classLevelId)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = "Student is already assigned to this class",
                    Data = null
                };
            }

            user.ClassLevelId = classLevelId;
            await _context.SaveChangesAsync();
            return new BaseResponse
            {
                Status = true,
                Message = "Student Assigned successfully",
                Data = new
                {
                    StudentId = user.Id,
                    StudentName = user.UserName,
                    ClassLevelId = classLevel.Id,
                    ClassLevelName = classLevel.Name
                }
            };
        }
    }
}
