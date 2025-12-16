using Blazored.LocalStorage;

namespace LockyLuke.Web.Utils
{
    public static class LocalStorageExtension
    {
        public async static Task<string> GetUserId(this ILocalStorageService LocalStorage)
        {
            return (await LocalStorage.GetItemAsStringAsync("userid"))?.ToString();

        }

        public static string GetUserIdSync(this ISyncLocalStorageService LocalStorage)
        {
            return LocalStorage.GetItemAsString("userid");

        }
    }
}
