namespace SharedLibrary.Dtos.User
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
