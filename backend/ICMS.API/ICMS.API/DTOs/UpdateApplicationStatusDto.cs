using ICMS.Models.Enums;

namespace ICMS.API.DTOs
{
    public class UpdateApplicationStatusDto
    {
        public ApplicationStatus Status { get; set; }
        public string? Remarks { get; set; }
    }
}