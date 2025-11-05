using BlogAppAPI.Data;
using BlogAppAPI.Models.Domain;
using BlogAppAPI.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

// Implementation for the Category interface, here I implement the CRUD methods.
namespace BlogAppAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        public CategoryRepository(ApplicationDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        public async Task Create(Category category)
        {
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
        }

        public int Count()
        {
            return _appDbContext.Categories.Count();
        }

        public async Task Delete(Guid id)
        {
            await _appDbContext.Categories.Where(category => category.Id == id).ExecuteDeleteAsync();
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Category>> Get()
        {
            return await _appDbContext.Categories.ToListAsync();
        }

        public async Task<Category> Get(Guid id)
        {
            return await _appDbContext.Categories.Where(category => category.Id == id).FirstOrDefaultAsync(); 
        }

        public async Task Update(Category category)
        {
            _appDbContext.Categories.Update(category);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
