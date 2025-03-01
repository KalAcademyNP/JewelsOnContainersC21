using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider) 
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var responseDto = await _authService.LoginAsync(loginRequestDto);
            if (responseDto != null && responseDto.IsSuccess)
            {
                var loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>
                    (Convert.ToString(responseDto.Result));

                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Catalog");
            } else
            {
                TempData["error"] = responseDto.Message;
                return View(loginRequestDto) ;
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Catalog");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                   jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                   jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                   jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                   jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));


            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            RegistrationRequestDto registrationRequestDto)
        {
            var result = await _authService.RegisterAsync(registrationRequestDto);
            if (result != null && result.IsSuccess)
            {
                TempData["success"] = "Registration successful!";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["error"] = result.Message;
            }
            return View(registrationRequestDto);
        }
    }

}
