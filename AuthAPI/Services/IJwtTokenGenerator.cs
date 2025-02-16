using AuthAPI.Models;

namespace AuthAPI.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, 
            IEnumerable<string> roles);
    }
}
