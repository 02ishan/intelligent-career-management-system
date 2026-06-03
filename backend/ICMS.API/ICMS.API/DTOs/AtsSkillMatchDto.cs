namespace ICMS.API.DTOs
{
    public class AtsSkillMatchDto
    {
        public int ApplicationId { get; set; }
        public List<string> MatchedSkills { get; set; } = new();
        public List<string> MissingSkills { get; set; } = new();
        public double Score { get; set; }
    }
}