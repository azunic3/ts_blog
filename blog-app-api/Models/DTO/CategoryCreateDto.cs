namespace BlogAppAPI.Models.DTO
{
    public class CategoryCreateDto
    {
        public required string Name { get; set; }
        public required string UrlHandle { get; set; }
    }
}
