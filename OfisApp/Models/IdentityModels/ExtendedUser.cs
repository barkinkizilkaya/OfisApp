using Microsoft.AspNetCore.Identity;

namespace OfisApp.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        public long CardNumber { get; set; }
    }
}
