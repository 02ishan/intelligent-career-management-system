namespace ICMS.API.DTOs
{
    public class AtsScoreDto
    {
        public int ApplicationId { get; set; }
        public int MatchedKeywords { get; set; }
        public int TotalKeywords { get; set; }
        public double Score { get; set; }
    }
}