using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using BlogAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlogAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Login")]
        [SwaggerOperation("Login for Access Token")]
        [AllowAnonymous]

        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authRepository.LoginAsync(user.Username, user.Password);

            if (result == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = await _authRepository.GenerateJwtTokenAsync(result);
            return Ok(new {Token = token});
        }

        [HttpPost("Register")]
        [SwaggerOperation("Register User")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCreateDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = new ApplicationUser
            {
                UserName = user.Username,
                Email = user.Email,
            };
            var result = await _authRepository.RegisterAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var token = await _authRepository.GenerateJwtTokenAsync(newUser);
            return Ok(new { Token = token });
        }
    }
}
