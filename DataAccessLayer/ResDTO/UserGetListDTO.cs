namespace DataAccessLayer.ResDTO
{
    public class UserGetListDTO
    {
        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string RoleSystem { get; set; } = null!;

        public string? Department { get; set; }

        public bool IsActive { get; set; }
    }
}
