using System.Text.Json.Serialization;

namespace BlogAppAPI.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string UrlHandle { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
