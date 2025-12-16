namespace LockyLuke.Web.Services
{
    public interface IChiperService
    {
        string Encrypt(string cipherText);
        string Decrypt(string cipherText);
    }
}
