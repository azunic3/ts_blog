using BlogAppAPI.Models.Domain;

namespace BlogAppAPI.Models.DTO
{
    public class BlogPostCreateDto
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string FeatureImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
        public bool IsVisible { get; set; }
        public List<Guid> Categories { get; set; }
    }
}
