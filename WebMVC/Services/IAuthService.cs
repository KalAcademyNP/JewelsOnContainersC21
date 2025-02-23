using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(
            RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssignRoleAsync(
            RegistrationRequestDto registrationRequestDto);
    }
}
