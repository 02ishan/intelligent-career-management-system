using ICMS.API.DTOs;
using ICMS.Data.Context;
using ICMS.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Recruiter,Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = new DashboardStatsDto
            {
                TotalJobs = await _context.Jobs.CountAsync(),

                TotalApplications = await _context.Applications.CountAsync(),

                ShortlistedCandidates = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Shortlisted),

                RejectedCandidates = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Rejected),

                SelectedCandidates = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Selected)
            };

            return Ok(stats);
        }

        [HttpGet("recent-applications")]
        public async Task<IActionResult> GetRecentApplications()
        {
            var recentApplications = await _context.Applications
                .OrderByDescending(a => a.AppliedDate)
                .Take(5)
                .ToListAsync();

            return Ok(recentApplications);
        }

        [HttpGet("application-status-summary")]
        public async Task<IActionResult> GetApplicationStatusSummary()
        {
            var summary = new ApplicationStatusSummaryDto
            {
                Applied = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Applied),

                Shortlisted = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Shortlisted),

                Rejected = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Rejected),

                Selected = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Selected)
            };

            return Ok(summary);
        }
    }
}