using BlogAppAPI.Models.Domain;

namespace BlogAppAPI.Models.DTO
{
    public class CategoryGetResponseDto
    {   
        public int Page { get; set; }
        public List<Category> Categories { get; set; }
    }
}
