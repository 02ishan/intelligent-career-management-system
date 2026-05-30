using ICMS.Data.Context;
using ICMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> CreateJob(Job job)
        {
            await _context.Jobs.AddAsync(job);

            await _context.SaveChangesAsync();

            return Ok(job);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, Job updatedJob)
        {
            var existingJob = await _context.Jobs.FindAsync(id);

            if (existingJob == null)
            {
                return NotFound();
            }

            existingJob.Title = updatedJob.Title;
            existingJob.CompanyName = updatedJob.CompanyName;
            existingJob.Location = updatedJob.Location;
            existingJob.Description = updatedJob.Description;
            existingJob.PostedDate = updatedJob.PostedDate;

            await _context.SaveChangesAsync();

            return Ok(existingJob);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);

            await _context.SaveChangesAsync();

            return Ok("Job deleted successfully");
        }
    }
}