using ICMS.Models.Enums;

namespace ICMS.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobId { get; set; }
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
        public string? Remarks { get; set; }
    }
}