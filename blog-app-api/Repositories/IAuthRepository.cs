using BlogAppAPI.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace BlogAppAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> LoginAsync(string username, string password);
        Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
    }
}
