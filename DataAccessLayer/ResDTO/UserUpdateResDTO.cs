namespace DataAccessLayer.ReqDTO
{
    public class UserUpdateResDTO
    {
        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
