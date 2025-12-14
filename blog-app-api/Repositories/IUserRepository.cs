using BlogAppAPI.Models.DTO;

namespace BlogAppAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<List<UserGetDto>> GetAllAsync();
        Task<bool> ExistsAsync(string id);
        Task DeleteAsync(string id);
    }
}
