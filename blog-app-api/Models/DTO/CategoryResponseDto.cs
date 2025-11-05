using BlogAppAPI.Models.Domain;

public class CategoryResponseDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public Category Data { get; set; }
}