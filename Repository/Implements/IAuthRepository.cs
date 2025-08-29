using DataAccessLayer.ReqDTO;
using DataAccessLayer.ResDTO;

namespace Repository.Implements
{
    public interface IAuthRepository
    {
        Task<LoginResDTO> Login(LoginReqDTO req);
        Task<LoginResDTO> Register(RegisterReqDTO req);
        Task<UserUpdateResDTO> UserUpdate(int id, UserUpdateReqDTO req);
    }
}
