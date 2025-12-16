namespace SharedLibrary.Dtos.Auth
{

    public class TokenDto
    {
        public string accessToken { get; set; }
        public DateTime accessTokenExpire { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpire { get; set; }
        public string userId { get; set; }
    }


}
