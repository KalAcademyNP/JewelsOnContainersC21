using AuthAPI.Models.Dto;
using AuthAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        protected ResponseDto _response;
        public AuthAPIController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _configuration = config;
            _response = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegistrationRequestDto model)
        {
            var response = await _authService.Register(model);
            if(!response.IsSuccess)
            {
                _response.IsSuccess = false;
                _response.Message = response.Message;
                return BadRequest(_response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(
            [FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(
                        model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error assigning user to the role";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
