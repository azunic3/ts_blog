using BlogAppAPI.Models.Domain;

namespace BlogAppAPI.Models.DTO
{
    public class BlogPostGetResponseDto
    {
        public int Page { get; set; }
        public List<BlogPostGetDto> BlogPosts { get; set; }
    }
}
