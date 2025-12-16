using Microsoft.AspNetCore.DataProtection;

namespace LockyLuke.Web.Services
{
    public class ChiperService : IChiperService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "forhonorconqwarden";
        public ChiperService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }
        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Unprotect(cipherText);
        }

        public string Encrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(cipherText);
        }
    }
}
