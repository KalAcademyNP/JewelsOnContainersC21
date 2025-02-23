using System.Security.Principal;

namespace WebMVC.Services
{
    public interface IIdentityService<T>
    {
        T Get(IPrincipal principal);
    }
}
