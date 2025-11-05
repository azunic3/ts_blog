using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;

namespace BlogAppAPI.Repositories
{
    public interface IBlogRepository
    {
        public Task<List<BlogPostGetDto>> GetVisible(int page = 1);
        public Task<List<BlogPostGetDto>> Get(int page = 1);
        public Task<BlogPostGetDto> Get(Guid id);
        public Task<BlogPostGetDto> GetByUrl(string urlHandle);
        public Task Create(BlogPostCreateDto blogPost);
        public Task Update(Guid id, BlogPostCreateDto blogPost);
        public Task Delete(Guid id);
        public int Count();
        public int CountVisible();
    }
}
