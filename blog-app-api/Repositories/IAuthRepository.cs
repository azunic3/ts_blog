using Microsoft.AspNetCore.Identity;

namespace BlogAppAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<IdentityUser> LoginAsync(string username, string password);
        Task<IdentityResult> RegisterAsync(IdentityUser user, string password);
        Task<string> GenerateJwtTokenAsync(IdentityUser user);
    }
}
