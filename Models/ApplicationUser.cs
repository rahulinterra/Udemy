using Microsoft.AspNetCore.Identity;

namespace CRUD.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
