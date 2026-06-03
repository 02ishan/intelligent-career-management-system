using ICMS.Data.Context;
using Microsoft.EntityFrameworkCore;
using ICMS.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;
using System.Text;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Recruiter,Admin")]
    public class ATSController : ControllerBase
    {
        private static readonly string[] StopWords =
        {
            "work",
            "on",
            "and",
            "the",
            "a",
            "an",
            "for",
            "with",
            "in",
            "to",
            "of",
            "applications",
            "application"
        };
        private readonly ApplicationDbContext _context;
        public ATSController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("score/{applicationId}")]
        public async Task<IActionResult> GetScore(int applicationId)
        {
            var application = await _context.Applications.FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
            {
                return NotFound("Application not found.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == application.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == application.JobId);

            if (job == null)
            {
                return NotFound("Job not found.");
            }

            var resumePath = Path.Combine(Directory.GetCurrentDirectory(), user.ResumePath!);

            var textBuilder = new StringBuilder();

            using (var document = PdfDocument.Open(resumePath))
            {
                foreach (var page in document.GetPages())
                {
                    textBuilder.AppendLine(page.Text);
                }
            }

            var resumeText = textBuilder.ToString();

            var keywords = job.Description!
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(k => !StopWords.Contains(k.ToLower()))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

            var matchedSkills = keywords.Where(keyword =>
                resumeText.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            var missingSkills = keywords.Except(matchedSkills, StringComparer.OrdinalIgnoreCase).ToList();

            var score = keywords.Length == 0
                ? 0
                : (double)matchedSkills.Count / keywords.Length * 100;

            var result = new AtsSkillMatchDto
            {
                ApplicationId = applicationId,
                MatchedSkills = matchedSkills,
                MissingSkills = missingSkills,
                Score = Math.Round(score, 2)
            };

            return Ok(result);
        }
    }
}