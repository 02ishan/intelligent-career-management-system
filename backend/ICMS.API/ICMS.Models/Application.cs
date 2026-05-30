namespace ICMS.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobId { get; set; }
        public DateTime AppliedDate { get; set; }
    }
}