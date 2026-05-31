using System.Security.Claims;
using ICMS.API.DTOs;
using ICMS.Data.Context;
using ICMS.Models;
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

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role == "Student")
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var applications = await _context.Applications.Where(a => a.UserId == user.Id).ToListAsync();

                return Ok(applications);
            }

            var allApplications = await _context.Applications.ToListAsync();

            return Ok(allApplications);
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

            await _context.SaveChangesAsync();

            return Ok(application);
        }
    }
}