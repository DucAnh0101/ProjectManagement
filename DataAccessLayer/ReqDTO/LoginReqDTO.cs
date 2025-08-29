namespace DataAccessLayer.ReqDTO
{
    public class LoginReqDTO
    {

        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
    }
}
