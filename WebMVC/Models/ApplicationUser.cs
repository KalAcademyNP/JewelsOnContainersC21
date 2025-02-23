using Microsoft.AspNetCore.Identity;

namespace WebMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
