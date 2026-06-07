namespace ICMS.Models
{
    public class JobAudit
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int RecruiterId { get; set; }
        public string RecruiterEmail { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}