namespace DataAccessLayer.ReqDTO
{
    public class UserUpdateReqDTO
    {
        public string PasswordHash { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
