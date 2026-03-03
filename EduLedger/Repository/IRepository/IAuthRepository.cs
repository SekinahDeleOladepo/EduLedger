using EduLedger.Data.DTOs.UserDTOs;
using EduLedger.Entitites.DTOs.AuthDTO;
using EduLedger.Entitites.Models;

namespace EduLedger.Repository.IRepository
{
    public interface IAuthRepository
    {
        Task<BaseResponse> CreateUser(CreateUserDTO createUser);
        Task<BaseResponse> LoginAsync(LoginRequestDTO logIN);
        Task<BaseResponse> ForgotPasswordAsync(ForgotPassword dto);
        Task<BaseResponse> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<BaseResponse> DeleteUser(DeleteUserDTO deleteUser);
        Task<BaseResponse> GetAllUser(string name);
        Task<BaseResponse> RegisterAsync(RegisterUserDTO dto);
        Task<BaseResponse> GetUser(int Id);
        
            
            
            
        

    }
}
