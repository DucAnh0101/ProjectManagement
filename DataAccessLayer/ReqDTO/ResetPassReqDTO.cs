namespace DataAccessLayer.ReqDTO
{
    public class ResetPassReqDTO
    {
        public string Username { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
