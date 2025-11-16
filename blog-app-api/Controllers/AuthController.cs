using BlogAppAPI.Data;
using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using BlogAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            IAuthRepository authRepository,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _authRepository = authRepository;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var foundUser = await _authRepository.LoginAsync(user.Username, user.Password);

            if (foundUser == null)
                return Unauthorized("Invalid username or password.");

            var (accessToken, refreshToken) = await _authRepository.GenerateTokensAsync(foundUser);

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            SetRefreshTokenCookie(refreshToken);

            return Ok(new { token = accessToken });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new ApplicationUser
            {
                UserName = user.Username,
                Email = user.Email,
            };

            var result = await _authRepository.RegisterAsync(newUser, user.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Automatically log in after register
            var (accessToken, refreshToken) = await _authRepository.GenerateTokensAsync(newUser);

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            SetRefreshTokenCookie(refreshToken);

            return Ok(new { token = accessToken });
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return Unauthorized("Missing refresh token");

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (storedToken == null || !storedToken.IsActive)
                return Unauthorized("Invalid refresh token");

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
                return Unauthorized();

            // Revoke the old refresh token
            storedToken.Revoked = DateTime.UtcNow;

            var (newAccessToken, newRefreshToken) = await _authRepository.GenerateTokensAsync(user);

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            SetRefreshTokenCookie(newRefreshToken);

            return Ok(new { token = newAccessToken });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken != null)
            {
                var storedToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(x => x.Token == refreshToken);

                if (storedToken != null)
                {
                    storedToken.Revoked = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                Response.Cookies.Delete("refreshToken");
            }

            return Ok("Logged out.");
        }


        //helper method
        private void SetRefreshTokenCookie(RefreshToken token)
        {
            Response.Cookies.Append("refreshToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = token.Expires
            });
        }
    }
}
