namespace BlogAppAPI.Models.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Foreign key to BlogPost
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        // Foreign key to AspNetUser
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
