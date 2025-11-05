namespace BlogAppAPI.Models.DTO
{
    public class ErrorResponseDto
    {
        public string Detail { get; set; }
        public ErrorResponseDto(string detail)
        {
            Detail = detail;
        }
    }
}
