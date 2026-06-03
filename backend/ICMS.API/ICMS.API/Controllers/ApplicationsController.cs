using System.Security.Claims;
using ICMS.API.DTOs;
using ICMS.Data.Context;
using ICMS.Models;
using ICMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public ApplicationsController(
            ApplicationDbContext context,
            EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);

            IQueryable<Application> query = _context.Applications;

            if (role == "Student")
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                query = query.Where(a => a.UserId == user.Id);
            }

            var applications = await query.ToListAsync();

            var result = new List<ApplicationDetailsDto>();

            foreach (var application in applications)
            {
                var user = await _context.Users.FindAsync(application.UserId);

                var job = await _context.Jobs.FindAsync(application.JobId);

                if (user == null || job == null)
                {
                    continue;
                }

                result.Add(new ApplicationDetailsDto
                {
                    ApplicationId = application.Id,
                    CandidateName = user.FullName ?? string.Empty,
                    JobTitle = job.Title ?? string.Empty,
                    CompanyName = job.CompanyName ?? string.Empty,
                    Status = application.Status.ToString(),
                    AppliedDate = application.AppliedDate
                });
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateApplication(CreateApplicationDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var application = new Application
            {
                UserId = user.Id,
                JobId = dto.JobId,
                AppliedDate = DateTime.UtcNow
            };

            await _context.Applications.AddAsync(application);

            await _context.SaveChangesAsync();

            return Ok(application);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateApplication(int id, Application updatedApplication)
        {
            var existingApplication = await _context.Applications.FindAsync(id);

            if (existingApplication == null)
            {
                return NotFound();
            }

            existingApplication.UserId = updatedApplication.UserId;
            existingApplication.JobId = updatedApplication.JobId;
            existingApplication.AppliedDate = updatedApplication.AppliedDate;

            await _context.SaveChangesAsync();

            return Ok(existingApplication);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);

            await _context.SaveChangesAsync();

            return Ok("Application deleted successfully");
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, UpdateApplicationStatusDto dto)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            application.Status = dto.Status;
            application.Remarks = dto.Remarks;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == application.UserId);

            if (user != null)
            {
                await _emailService.SendEmailAsync(
                    user.Email!,
                    "Application Status Updated",
                    $"Hello {user.FullName}, your application status has been updated to {application.Status}.");
            }

            await _context.SaveChangesAsync();

            return Ok(application);
        }
    }
}