using Newtonsoft.Json;
using WebMVC.Infrastructure;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _baseUrl;
        private readonly IHttpClient _httpClient;
        public AuthService(IConfiguration config, IHttpClient client) 
        {
            _baseUrl = $"{config["IdentityUrl"]}/api/auth";
            _httpClient = client;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            var assignRoleUri = APIPaths.Auth.AssignRole(_baseUrl);
            var response = await _httpClient.PostAsync(assignRoleUri, 
                registrationRequestDto);
            return JsonConvert.DeserializeObject<ResponseDto>(response.Content.ToString());
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var loginUri = APIPaths.Auth.Login(_baseUrl);
            var response = await _httpClient.PostAsync(loginUri, loginRequestDto);
            var dataString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseDto>(dataString);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            var registerUri = APIPaths.Auth.Register(_baseUrl);
            var response = await _httpClient.PostAsync(registerUri, registrationRequestDto);
            var dataString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseDto>(dataString);
        }
    }
}
