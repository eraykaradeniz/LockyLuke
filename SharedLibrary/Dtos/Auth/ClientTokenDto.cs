namespace SharedLibrary.Dtos.Auth
{
    public class ClientTokenDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpire { get; set; }
    }
}
