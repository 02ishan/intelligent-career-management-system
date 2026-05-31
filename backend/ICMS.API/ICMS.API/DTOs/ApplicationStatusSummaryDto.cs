namespace ICMS.API.DTOs
{
    public class ApplicationStatusSummaryDto
    {
        public int Applied { get; set; }
        public int Shortlisted { get; set; }
        public int Rejected { get; set; }
        public int Selected { get; set; }
    }
}