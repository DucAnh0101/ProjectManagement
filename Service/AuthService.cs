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

        public Task<UserGetListDTO> AdminUpdateUser(int id, AdminUpdateUserReqDTO req)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            try
            {
                return _authRepository.AdminUpdateUser(id, req);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi trong quá trình cập nhật thông tin người dùng bởi admin.", ex);
            }
        }

        public Task<IEnumerable<UserGetListDTO>> GetListUser()
        {
            if (_authRepository == null)
            {
                throw new InvalidOperationException("Repository không được khởi tạo.");
            }

            try
            {
                return _authRepository.GetListUser();
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Đã xảy ra lỗi trong quá trình lấy danh sách người dùng.", ex);
            }
        }

        public Task<UserGetListDTO> GetUserById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            try
            {
                return _authRepository.GetUserById(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoginResDTO> Login(LoginReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException("Thông tin đăng nhập không được để trống.");
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
                throw new ArgumentNullException("Thông tin đăng ký không được để trống.");
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

        public Task ResetPassword(ResetPassReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException("Thông tin đặt lại mật khẩu không được để trống.");
            }
            try
            {
                return _authRepository.ResetPassword(req);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserUpdateResDTO> UserUpdate(int id, UserUpdateReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException("Thông tin cập nhật không được để trống.");
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