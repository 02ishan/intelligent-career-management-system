namespace ICMS.API.DTOs
{
    public class ApplicationDetailsDto
    {
        public int ApplicationId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; }
    }
}