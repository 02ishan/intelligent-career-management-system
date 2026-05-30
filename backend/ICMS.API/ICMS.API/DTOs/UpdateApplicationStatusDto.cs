namespace ICMS.API.DTOs
{
    public class UpdateApplicationStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string? Remarks { get; set; }
    }
}