using BlogAppAPI.Data;
using BlogAppAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BlogAppAPI.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public BlogRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<BlogPostGetDto>> GetMine(int page, string username, bool isAdmin)
        {
            if (page < 1) page = 1;

            var query = _appDbContext.BlogPosts
                .Include(x => x.Categories)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(x => x.Author == username);

            return await query
                .OrderByDescending(x => x.PublishDate)
                .Skip((page - 1) * 6)
                .Take(6)
                .Select(x => new BlogPostGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Content = x.Content,
                    FeatureImageUrl = x.FeatureImageUrl,
                    UrlHandle = x.UrlHandle,
                    PublishDate = x.PublishDate,
                    Author = x.Author,
                    IsVisible = x.IsVisible,
                    Categories = x.Categories.Select(c => new CategoryGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<int> CountMine(string username, bool isAdmin)
        {
            var query = _appDbContext.BlogPosts.AsQueryable();

            if (!isAdmin)
                query = query.Where(x => x.Author == username);

            return await query.CountAsync();
        }

        public async Task<List<BlogPostGetDto>> GetVisible(int page = 1)
        {
            if (page < 1) page = 1;

            return await _appDbContext.BlogPosts
                .Include(x => x.Categories)
                .Where(x => x.IsVisible == true)
                .OrderByDescending(x => x.PublishDate)
                .Skip((page - 1) * 6)
                .Take(6)
                .Select(x => new BlogPostGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Content = x.Content,
                    FeatureImageUrl = x.FeatureImageUrl,
                    UrlHandle = x.UrlHandle,
                    PublishDate = x.PublishDate,
                    Author = x.Author,
                    IsVisible = x.IsVisible,
                    Categories = x.Categories.Select(c => new CategoryGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<BlogPostGetDto>> Get(int page = 1)
        {
            if (page < 1) page = 1;

            return await _appDbContext.BlogPosts
                .Include(x => x.Categories)
                .OrderByDescending(x => x.PublishDate)
                .Skip((page - 1) * 6)
                .Take(6)
                .Select(x => new BlogPostGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Content = x.Content,
                    FeatureImageUrl = x.FeatureImageUrl,
                    UrlHandle = x.UrlHandle,
                    PublishDate = x.PublishDate,
                    Author = x.Author,
                    IsVisible = x.IsVisible,
                    Categories = x.Categories.Select(c => new CategoryGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<BlogPostGetDto?> Get(Guid id)
        {
            return await _appDbContext.BlogPosts
                .Include(x => x.Categories)
                .Where(x => x.Id == id)
                .Select(x => new BlogPostGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Content = x.Content,
                    FeatureImageUrl = x.FeatureImageUrl,
                    UrlHandle = x.UrlHandle,
                    PublishDate = x.PublishDate,
                    Author = x.Author,
                    IsVisible = x.IsVisible,
                    Categories = x.Categories.Select(c => new CategoryGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<BlogPostGetDto?> GetByUrl(string urlHandle)
        {
            return await _appDbContext.BlogPosts
                .Include(x => x.Categories)
                .Include(x => x.Comments)
                    .ThenInclude(c => c.User)
                .Where(x => x.UrlHandle == urlHandle)
                .Select(x => new BlogPostGetDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Content = x.Content,
                    FeatureImageUrl = x.FeatureImageUrl,
                    UrlHandle = x.UrlHandle,
                    PublishDate = x.PublishDate,
                    Author = x.Author,
                    IsVisible = x.IsVisible,
                    Categories = x.Categories.Select(c => new CategoryGetDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        UrlHandle = c.UrlHandle
                    }).ToList(),
                    Comments = x.Comments.Select(c => new CommentGetDto
                    {
                        Id = c.Id,
                        Content = c.content,
                        CreatedAt = c.CreatedAt,
                        UserName = c.User.UserName
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public int Count() => _appDbContext.BlogPosts.Count();

        public int CountVisible() => _appDbContext.BlogPosts.Count(x => x.IsVisible);

        public async Task Create(BlogPostCreateDto blogPost)
        {
            var categories = await _appDbContext.Categories
                .Where(category => blogPost.Categories.Contains(category.Id))
                .ToListAsync();

            var newBlogPost = new Models.Domain.BlogPost
            {
                Title = blogPost.Title,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeatureImageUrl = blogPost.FeatureImageUrl,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                Author = blogPost.Author,          
                PublishDate = blogPost.PublishDate,
                Categories = categories
            };

            await _appDbContext.BlogPosts.AddAsync(newBlogPost);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await _appDbContext.BlogPosts
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            await _appDbContext.SaveChangesAsync();
        }

        public async Task Update(Guid id, BlogPostCreateDto blogPost)
        {
            var existingBlogPost = await _appDbContext.BlogPosts
                .Include(bp => bp.Categories)
                .FirstOrDefaultAsync(bp => bp.Id == id);

            if (existingBlogPost == null)
                throw new KeyNotFoundException("BlogPost not found.");

            existingBlogPost.Title = blogPost.Title;
            existingBlogPost.ShortDescription = blogPost.ShortDescription;
            existingBlogPost.Content = blogPost.Content;
            existingBlogPost.FeatureImageUrl = blogPost.FeatureImageUrl;
            existingBlogPost.UrlHandle = blogPost.UrlHandle;
            existingBlogPost.PublishDate = blogPost.PublishDate;
            existingBlogPost.Author = blogPost.Author; 
            existingBlogPost.IsVisible = blogPost.IsVisible;

            existingBlogPost.Categories.Clear();

            var newCategories = await _appDbContext.Categories
                .Where(category => blogPost.Categories.Contains(category.Id))
                .ToListAsync();

            foreach (var category in newCategories)
                existingBlogPost.Categories.Add(category);

            await _appDbContext.SaveChangesAsync();
        }
    }
}
