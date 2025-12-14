namespace BlogAppAPI.Models.DTO
{
    public class BlogPostCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string FeatureImageUrl { get; set; } = string.Empty;
        public string UrlHandle { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public string? Author { get; set; }
        public bool IsVisible { get; set; }
        public List<Guid> Categories { get; set; } = new();
    }
}
