namespace DataAccessLayer.ReqDTO
{
    public class AdminUpdateUserReqDTO
    {
        public string RoleSystem { get; set; } = null!;

        public string? Department { get; set; }

        public bool IsActive { get; set; }
    }
}
