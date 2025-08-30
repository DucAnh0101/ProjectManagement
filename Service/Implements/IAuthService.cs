using DataAccessLayer.ReqDTO;
using DataAccessLayer.ResDTO;

namespace Service.Implements
{
    public interface IAuthService
    {
        Task<LoginResDTO> Login(LoginReqDTO req);
        Task<LoginResDTO> Register(RegisterReqDTO req);
        Task<UserUpdateResDTO> UserUpdate(int id, UserUpdateReqDTO req);
        Task<IEnumerable<UserGetListDTO>> GetListUser();
        Task<UserGetListDTO> GetUserById(int id);
        Task<UserGetListDTO> AdminUpdateUser(int id, AdminUpdateUserReqDTO req);
        Task ResetPassword(ResetPassReqDTO req);
    }
}