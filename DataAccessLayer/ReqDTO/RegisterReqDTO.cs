namespace DataAccessLayer.ReqDTO
{
    public class RegisterReqDTO
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
