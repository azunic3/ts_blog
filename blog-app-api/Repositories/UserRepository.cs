using BlogAppAPI.Data;
using BlogAppAPI.Models.DTO;
using BlogAppAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogAppAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserGetDto>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.UserName)
                .Select(u => new UserGetDto
                {
                    Id = u.Id,
                    Username = u.UserName!,
                    Email = u.Email
                })
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
