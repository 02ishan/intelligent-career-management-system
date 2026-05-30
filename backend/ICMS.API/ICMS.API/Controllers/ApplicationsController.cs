using ICMS.Data.Context;
using ICMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var applications = await _context.Applications.ToListAsync();

            return Ok(applications);
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplication(Application application)
        {
            await _context.Applications.AddAsync(application);

            await _context.SaveChangesAsync();

            return Ok(application);
        }

        [HttpPut("{id}")]
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
    }
}