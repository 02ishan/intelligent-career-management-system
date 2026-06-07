using ICMS.API.DTOs;
using ICMS.API.Services;
using ICMS.Data.Context;
using ICMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditLogService _auditLogService;

        public JobsController(
            ApplicationDbContext context,
            AuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _context.Jobs.ToListAsync();

            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        [HttpPost]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> CreateJob(CreateJobDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var recruiter = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (recruiter == null)
            {
                return NotFound("Recruiter not found.");
            }

            var job = new Job
            {
                Title = dto.Title,
                CompanyName = dto.CompanyName,
                Location = dto.Location,
                Description = dto.Description,
                PostedDate = DateTime.UtcNow,
                RecruiterId = recruiter.Id
            };

            await _context.Jobs.AddAsync(job);

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                recruiter.Id,
                recruiter.Email ?? "",
                "JOB_CREATED",
                "Job",
                job.Id,
                $"Created job {job.Title}"
            );

            await _auditLogService.LogJobAsync(
                job,
                recruiter.Email ?? "",
                "JOB_CREATED"
            );

            return Ok(job);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> UpdateJob(int id, Job updatedJob)
        {
            var existingJob = await _context.Jobs.FindAsync(id);

            if (existingJob == null)
            {
                return NotFound();
            }

            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role == "Recruiter")
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var recruiter = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (recruiter == null)
                {
                    return NotFound("Recruiter not found.");
                }

                if (existingJob.RecruiterId != recruiter.Id)
                {
                    return Forbid();
                }
            }

            existingJob.Title = updatedJob.Title;
            existingJob.CompanyName = updatedJob.CompanyName;
            existingJob.Location = updatedJob.Location;
            existingJob.Description = updatedJob.Description;

            await _context.SaveChangesAsync();

            var recruiterEmail = User.FindFirstValue(ClaimTypes.Email) ?? "";

            await _auditLogService.LogAsync(
                existingJob.RecruiterId,
                recruiterEmail,
                "JOB_UPDATED",
                "Job",
                existingJob.Id,
                $"Updated job {existingJob.Title}"
            );

            await _auditLogService.LogJobAsync(
                existingJob,
                recruiterEmail,
                "JOB_UPDATED"
            );

            return Ok(existingJob);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role == "Recruiter")
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var recruiter = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (recruiter == null)
                {
                    return NotFound("Recruiter not found.");
                }

                if (job.RecruiterId != recruiter.Id)
                {
                    return Forbid();
                }
            }

            var recruiterEmail = User.FindFirstValue(ClaimTypes.Email) ?? "";

            await _auditLogService.LogAsync(
                job.RecruiterId,
                recruiterEmail,
                "JOB_DELETED",
                "Job",
                job.Id,
                $"Deleted job {job.Title}"
            );

            await _auditLogService.LogJobAsync(
                job,
                recruiterEmail,
                "JOB_DELETED"
            );

            _context.Jobs.Remove(job);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Job deleted successfully"
            });
        }
    }
}