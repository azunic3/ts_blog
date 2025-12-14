using BlogAppAPI.Models.DTO;

namespace BlogAppAPI.Repositories
{
    public interface IBlogRepository
    {
        Task<List<BlogPostGetDto>> GetVisible(int page = 1);
        Task<List<BlogPostGetDto>> Get(int page = 1);

        Task<BlogPostGetDto?> Get(Guid id);
        Task<BlogPostGetDto?> GetByUrl(string urlHandle);

        Task Create(BlogPostCreateDto blogPost);
        Task Update(Guid id, BlogPostCreateDto blogPost);
        Task Delete(Guid id);

        int Count();
        int CountVisible();
        Task<List<BlogPostGetDto>> GetMine(int page, string username, bool isAdmin);
        Task<int> CountMine(string username, bool isAdmin);
    }
}
