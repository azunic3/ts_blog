namespace BlogAppAPI.Models.DTO
{
    public class UserGetDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
