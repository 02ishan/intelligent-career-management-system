using System.Security.Claims;
using ICMS.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using ICMS.Data.Context;
using ICMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _context.Jobs.ToListAsync();

            return Ok(jobs);
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

            _context.Jobs.Remove(job);

            await _context.SaveChangesAsync();

            return Ok("Job deleted successfully");
        }
    }
}