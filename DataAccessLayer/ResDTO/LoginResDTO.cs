namespace DataAccessLayer.ResDTO
{
    public class LoginResDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string RoleSystem { get; set; } = null!;
    }
}
