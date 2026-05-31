namespace ICMS.API.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalJobs { get; set; }
        public int TotalApplications { get; set; }
        public int ShortlistedCandidates { get; set; }
        public int RejectedCandidates { get; set; }
        public int SelectedCandidates { get; set; }
    }
}