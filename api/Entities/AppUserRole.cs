using Microsoft.AspNetCore.Identity;

namespace api.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }

        public AppRole Role { get; set; }
    }
}