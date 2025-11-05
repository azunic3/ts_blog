using BlogAppAPI.Models.Domain;

namespace BlogAppAPI.Repositories.Interface
{
    public interface IBlogImageRepository
    {
        Task SaveImage(BlogImage image);

        Task<IEnumerable<BlogImage>> GetAllImages();
    }
}
