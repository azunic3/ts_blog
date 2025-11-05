using BlogAppAPI.Models.DTO;
using System.Text.Json.Serialization;

namespace BlogAppAPI.Models.Domain
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string ShortDescription { get; set; }
        public required string Content { get; set; }
        public required string FeatureImageUrl { get; set; }
        public required string UrlHandle { get; set; }
        public DateTime PublishDate { get; set; }
        public required string Author {  get; set; }
        public bool IsVisible { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
