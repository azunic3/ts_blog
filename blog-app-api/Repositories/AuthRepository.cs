using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAppAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<IdentityUser> _userManager; // Provided function by ASP.NET for user management operations.
        private readonly IConfiguration _configuration; // For appsettings.json access.
        private readonly SignInManager<IdentityUser> _signInManager; // For sign in management operations.

        public AuthRepository(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<IdentityUser> LoginAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return await _userManager.FindByNameAsync(username);
            }

            return null;
        }

        // Method that generates a JWT token.
        public async Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            // User specific data used in the token payload. Here we use username, user id and adding Jti, a id for the token.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id)
            };

            // Add role 
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate the token based on the user data, token config in appsettings.json, key and credentials.
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Method that registers and stores a new user into the database with the provided 'IdentityUser' object and password.
        public async Task<IdentityResult> RegisterAsync(IdentityUser user, string password)
        {
            
            var createUser = await _userManager.CreateAsync(user, password);

            if (!createUser.Succeeded)
            {
                return createUser;
            }

            var assignRole = await _userManager.AddToRoleAsync(user, "User");

            if (!assignRole.Succeeded)
            {
                return assignRole;
            }

            return createUser;
        }
    }
}
