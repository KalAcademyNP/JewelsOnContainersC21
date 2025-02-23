using System.Security.Claims;
using System.Security.Principal;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class IdentityService : IIdentityService<ApplicationUser>
    {
        public ApplicationUser Get(IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                var email = claims.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                var id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
                var user = new ApplicationUser
                {
                    Email = email,
                    Id = id
                };
                return user;
            }

            throw new ArgumentException(message: "The principal must be a claimsprincipal",
                paramName: nameof(principal));
        }
    }
}
