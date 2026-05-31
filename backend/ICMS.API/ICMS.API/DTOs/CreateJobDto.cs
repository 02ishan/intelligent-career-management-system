namespace ICMS.API.DTOs
{
    public class CreateJobDto
    {
        public string? Title { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
    }
}