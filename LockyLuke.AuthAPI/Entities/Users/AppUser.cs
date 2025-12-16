using Microsoft.AspNetCore.Identity;

namespace LockyLuke.AuthAPI.Entities.Users
{
    public class AppUser:IdentityUser
    {
        public AppUser()
        {
            InsertedDate = DateTime.Now;
            Status = true;
        }
        public bool Status { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
