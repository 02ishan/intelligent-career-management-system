using ICMS.API.DTOs;
using ICMS.API.Services;
using ICMS.Data.Context;
using ICMS.Models;
using ICMS.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        private async Task<List<ApplicationDetailsDto>> GetFilteredApplications(ApplicationStatus? status = null)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);

            IQueryable<Application> query = _context.Applications;

            if (role == "Student")
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user != null)
                {
                    query = query.Where(a => a.UserId == user.Id);
                }
            }
            else if (role == "Recruiter")
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var recruiter = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (recruiter != null)
                {
                    var recruiterJobIds = await _context.Jobs
                        .Where(j => j.RecruiterId == recruiter.Id)
                        .Select(j => j.Id)
                        .ToListAsync();

                    query = query.Where(a => recruiterJobIds.Contains(a.JobId));
                }
            }

            if (status != null)
            {
                query = query.Where(a => a.Status == status);
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

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);

            IQueryable<Application> query = _context.Applications;

            if (role == "Student")
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                query = query.Where(a => a.UserId == user.Id);
            }

            else if (role == "Recruiter")
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var recruiter = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (recruiter == null)
                {
                    return NotFound("Recruiter not found.");
                }

                var recruiterJobIds = await _context.Jobs
                    .Where(j => j.RecruiterId == recruiter.Id)
                    .Select(j => j.Id)
                    .ToListAsync();

                query = query.Where(a => recruiterJobIds.Contains(a.JobId));
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

        [HttpGet("shortlisted")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> GetShortlistedApplications()
        {
            return Ok(
                await GetFilteredApplications(
                    ApplicationStatus.Shortlisted
                )
            );
        }

        [HttpGet("selected")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> GetSelectedApplications()
        {
            return Ok(
                await GetFilteredApplications(
                    ApplicationStatus.Selected
                )
            );
        }

        [HttpGet("rejected")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> GetRejectedApplications()
        {
            return Ok(
                await GetFilteredApplications(
                    ApplicationStatus.Rejected
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateApplication(CreateApplicationDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var existingApplication =
                await _context.Applications
                .FirstOrDefaultAsync(a =>
                    a.UserId == user.Id &&
                    a.JobId == dto.JobId);

            if (existingApplication != null)
            {
                return BadRequest(new
                {
                    message = "You have already applied for this job."
                });
            }

            var application = new Application
            {
                UserId = user.Id,
                JobId = dto.JobId,
                AppliedDate = DateTime.UtcNow
            };

            await _context.Applications.AddAsync(application);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Application submitted successfully."
            });
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
                try
                {
                    await _emailService.SendEmailAsync(
                        user.Email!,
                        "Application Status Updated",
                        $"Hello {user.FullName}, your application status has been updated to {application.Status}.");
                }
                catch
                {
                    // Ignore email errors for now
                }
            }

            await _context.SaveChangesAsync();

            return Ok(application);
        }
    }
}