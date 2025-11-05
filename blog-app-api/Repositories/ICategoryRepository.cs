using BlogAppAPI.Models.Domain;

// This is only an interface, the implementation goes into a seperate file
namespace BlogAppAPI.Repositories
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> Get();
        public Task<Category> Get(Guid id);
        public Task Create(Category category);
        public Task Update(Category category);
        public Task Delete(Guid id);
        public int Count();
    }
}
