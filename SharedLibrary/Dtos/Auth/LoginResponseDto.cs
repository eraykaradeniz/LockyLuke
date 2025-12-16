using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos.Auth
{
    public class Data
    {
        public string accessToken { get; set; }
        public DateTime accessTokenExpire { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpire { get; set; }

        public string userId { get; set; }
    }

    public class LoginResponseDto
    {
        public Data data { get; set; }
        public int statusCode { get; set; }
        public object message { get; set; }
        public object error { get; set; }
    }
}
