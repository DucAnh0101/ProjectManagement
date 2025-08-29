using DataAccessLayer.ReqDTO;
using DataAccessLayer.ResDTO;
using Repository.Implements;
using Service.Implements;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<LoginResDTO> Login(LoginReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Thông tin đăng nhập không được để trống.");
            }

            try
            {
                return await _authRepository.Login(req);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi trong quá trình đăng nhập.", ex);
            }
        }

        public async Task<LoginResDTO> Register(RegisterReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Thông tin đăng ký không được để trống.");
            }

            try
            {
                return await _authRepository.Register(req);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi trong quá trình đăng ký.", ex);
            }
        }

        public async Task<UserUpdateResDTO> UserUpdate(int id, UserUpdateReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Thông tin cập nhật không được để trống.");
            }

            try
            {
                return await _authRepository.UserUpdate(id, req);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi trong quá trình cập nhật thông tin người dùng.", ex);
            }
        }
    }
}