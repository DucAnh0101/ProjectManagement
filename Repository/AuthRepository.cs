using BusinessObject.Models;
using DataAccessLayer.ReqDTO;
using DataAccessLayer.ResDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Implements;
using System.Net;
using System.Net.Mail;
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

        public async Task<UserGetListDTO> AdminUpdateUser(int id, AdminUpdateUserReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException(nameof(req), "Thông tin cập nhật không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(req.RoleSystem))
            {
                throw new ArgumentException("Vai trò hệ thống không được để trống.");
            }

            if (id <= 0)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            if (req.RoleSystem != "Admin" && req.RoleSystem != "Member" && req.RoleSystem != "Manager")
            {
                throw new ArgumentException("Vai trò hệ thống không hợp lệ. Chỉ chấp nhận 'Admin', 'Member' hoặc 'Manager'.");
            }

            if (req.Department != null && req.Department.Length > 100)
            {
                throw new ArgumentException("Tên phòng ban không được vượt quá 100 ký tự.");
            }

            if (req.RoleSystem == "Admin" && req.IsActive == false)
            {
                throw new ArgumentException("Không thể vô hiệu hóa tài khoản với vai trò Admin.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserId == id);

            if (user == null)
            {
                throw new ArgumentException("Người dùng không tồn tại.");
            }

            user.RoleSystem = req.RoleSystem;
            user.Department = req.Department;
            user.IsActive = req.IsActive;
            await _context.SaveChangesAsync();

            return new UserGetListDTO
            {
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                RoleSystem = user.RoleSystem,
                Department = user.Department,
                IsActive = user.IsActive
            };
        }

        public async Task<IEnumerable<UserGetListDTO>> GetListUser()
        {
            var users = _context.Users
                .Select(u => new UserGetListDTO
                {
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    RoleSystem = u.RoleSystem,
                    Department = u.Department,
                    IsActive = u.IsActive
                }).AsEnumerable();

            if (users == null)
            {
                throw new NullReferenceException("Không có người dùng nào trong hệ thống.");
            }

            return await Task.FromResult((IEnumerable<UserGetListDTO>)users);
        }

        public async Task<UserGetListDTO> GetUserById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID người dùng không hợp lệ.");
            }

            var u = _context.Users
                .Where(u => u.UserId == id);

            if (!u.Any())
            {
                throw new KeyNotFoundException("Người dùng không tồn tại.");
            }

            var user = _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserGetListDTO
                {
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    RoleSystem = u.RoleSystem,
                    Department = u.Department,
                    IsActive = u.IsActive
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Người dùng không tồn tại.");
            }

            return await user;
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
                UserId = user.UserId,
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

        public Task ResetPassword(ResetPassReqDTO req)
        {
            if (req == null)
            {
                throw new ArgumentNullException("Thông tin đặt lại mật khẩu không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(req.Username))
            {
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            }
            var user = _context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(req.Email) && !string.IsNullOrWhiteSpace(req.Phone))
            {
                user = user.Where(u => u.Username == req.Username && u.Email == req.Email && u.Phone == req.Phone);
            }
            else
            {
                throw new ArgumentException("Vui lòng cung cấp email và số điện thoại để xác thực.");
            }
            var existingUser = user.FirstOrDefault();
            if (existingUser == null)
            {
                throw new ArgumentException("Người dùng không tồn tại hoặc thông tin xác thực không đúng.");
            }

            var newPassword = GenerateRandomPassword();
            existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            _context.SaveChangesAsync();

            SendEmailAsync(existingUser.Email, "Reset password request", BuildResetPasswordEmailBody(existingUser.Username, newPassword));

            return Task.CompletedTask;
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

        private string GenerateRandomPassword(int length = 6)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            var rand = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = "bda2k3@gmail.com";
            var fromPassword = "buxi vval vqdf myls";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        private string BuildResetPasswordEmailBody(string userName, string newPassword)
        {
            return $@"
                    <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                background-color: #f9f9f9;
                padding: 20px;
                color: #333;
            }}
            .container {{
                background-color: #fff;
                padding: 20px;
                border-radius: 8px;
                box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                max-width: 600px;
                margin: auto;
            }}
            .highlight {{
                color: #0056b3;
                font-weight: bold;
            }}
            .footer {{
                margin-top: 30px;
                font-size: 12px;
                color: #888;
            }}
            </style>
                </head>
                     <body>
                        <div class='container'>
                            <h2>🔐 Yêu cầu đặt lại mật khẩu</h2>
                            <p>Xin chào <span class='highlight'>{userName}</span>,</p>
                            <p>Bạn hoặc ai đó đã yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
                            <p>Mật khẩu mới của bạn là:</p>
                            <p style='font-size: 18px; font-weight: bold; color: #d9534f;'>{newPassword}</p>
                            <p>Hãy đăng nhập và đổi mật khẩu ngay để đảm bảo an toàn.</p>
                            <p class='footer'>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này hoặc liên hệ bộ phận hỗ trợ.</p>
                        </div>
                    </body>
        </html>";
        }
    }
}