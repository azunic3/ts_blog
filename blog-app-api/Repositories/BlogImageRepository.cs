using BlogAppAPI.Data;
using BlogAppAPI.Models.Domain;
using BlogAppAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogAppAPI.Repositories
{
    public class BlogImageRepository : IBlogImageRepository
    {
        private readonly ApplicationDbContext _appDbcontext;

        public BlogImageRepository(ApplicationDbContext appDbcontext)
        {
            _appDbcontext = appDbcontext;
        }

        public async Task<IEnumerable<BlogImage>> GetAllImages()
        {
            return await _appDbcontext.BlogImages.ToListAsync();
        }


        public async Task SaveImage(BlogImage image)
        {
            await _appDbcontext.BlogImages.AddAsync(image);
            await _appDbcontext.SaveChangesAsync();
        }

    }
}
