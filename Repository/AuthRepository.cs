using BusinessObject.Models;
using DataAccessLayer.ReqDTO;
using DataAccessLayer.ResDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Implements;
using System.Text.RegularExpressions;

namespace Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ProjectManagementContext _context;

        public AuthRepository(ProjectManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<LoginResDTO> Login(LoginReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Thông tin đăng nhập không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.PasswordHash))
            {
                throw new ArgumentException("Tên đăng nhập và mật khẩu không được để trống.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == req.Username && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(req.PasswordHash, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Tên đăng nhập hoặc mật khẩu không hợp lệ.");
            }

            return new LoginResDTO
            {
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                RoleSystem = user.RoleSystem
            };
        }

        public async Task<LoginResDTO> Register(RegisterReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Thông tin đăng ký không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(req.Username) ||
                string.IsNullOrWhiteSpace(req.FullName) ||
                string.IsNullOrWhiteSpace(req.Email) ||
                string.IsNullOrWhiteSpace(req.Phone) ||
                string.IsNullOrWhiteSpace(req.Password) ||
                string.IsNullOrWhiteSpace(req.ConfirmPassword))
            {
                throw new ArgumentException("Vui lòng điền đầy đủ thông tin đăng ký.");
            }

            if (req.Password != req.ConfirmPassword)
            {
                throw new ArgumentException("Mật khẩu xác nhận không khớp.");
            }

            if (!Regex.IsMatch(req.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Định dạng email không hợp lệ.");
            }

            if (req.Password.Length < 8 || !Regex.IsMatch(req.Password, @"[A-Z]") ||
                !Regex.IsMatch(req.Password, @"[a-z]") || !Regex.IsMatch(req.Password, @"\d"))
            {
                throw new ArgumentException("Mật khẩu phải dài ít nhất 8 ký tự, chứa chữ hoa, chữ thường và số.");
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == req.Username.ToLower());

            if (existingUser != null)
            {
                throw new ArgumentException("Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
            }

            var existingEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == req.Email.ToLower());

            if (existingEmail != null)
            {
                throw new ArgumentException("Email đã được sử dụng, vui lòng chọn email khác.");
            }

            var newUser = new User
            {
                Username = req.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
                FullName = req.FullName,
                Email = req.Email,
                Phone = req.Phone,
                RoleSystem = "Member",
                IsActive = true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new LoginResDTO
            {
                UserId = newUser.UserId,
                Username = newUser.Username,
                FullName = newUser.FullName,
                Email = newUser.Email,
                Phone = newUser.Phone,
                RoleSystem = newUser.RoleSystem
            };
        }

        public async Task<UserUpdateResDTO> UserUpdate(int id, UserUpdateReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Thông tin cập nhật không được để trống.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserId == id);
            if (user == null)
            {
                throw new ArgumentException("Người dùng không tồn tại.");
            }

            if (string.IsNullOrWhiteSpace(req.FullName) ||
                string.IsNullOrWhiteSpace(req.Email) ||
                string.IsNullOrWhiteSpace(req.Phone))
            {
                throw new ArgumentException("Tên, email và số điện thoại không được để trống.");
            }

            if (!Regex.IsMatch(req.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Định dạng email không hợp lệ.");
            }

            var existingEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == req.Email.ToLower() && u.UserId != id);
            if (existingEmail != null)
            {
                throw new ArgumentException("Email đã được sử dụng, vui lòng chọn email khác.");
            }

            var existingPhone = await _context.Users
                .FirstOrDefaultAsync(u => u.Phone.ToLower() == req.Phone.ToLower() && u.UserId != id);
            if (existingPhone != null)
            {
                throw new ArgumentException("Số điện thoại đã được sử dụng, vui lòng chọn số khác.");
            }

            if (!string.IsNullOrWhiteSpace(req.PasswordHash))
            {
                if (req.PasswordHash != req.ConfirmPassword)
                {
                    throw new ArgumentException("Mật khẩu xác nhận không khớp.");
                }

                if (req.PasswordHash.Length < 8 || !Regex.IsMatch(req.PasswordHash, @"[A-Z]") ||
                    !Regex.IsMatch(req.PasswordHash, @"[a-z]") || !Regex.IsMatch(req.PasswordHash, @"\d"))
                {
                    throw new ArgumentException("Mật khẩu phải dài ít nhất 8 ký tự, chứa chữ hoa, chữ thường và số.");
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.PasswordHash);
            }

            user.FullName = req.FullName;
            user.Email = req.Email;
            user.Phone = req.Phone;
            await _context.SaveChangesAsync();

            return new UserUpdateResDTO
            {
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone
            };
        }
    }
}