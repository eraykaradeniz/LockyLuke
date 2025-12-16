using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    public static class SignService
    {
        public static SecurityKey GetSecurityKey(string key)
        {
            return new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
        }
    }
}
