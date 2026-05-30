using ICMS.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResumeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResumeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UploadResume(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload a file.");
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var fileName = $"{user.Id}_{Path.GetFileName(file.FileName)}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Resumes");
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            user.ResumePath = Path.Combine("Uploads", "Resumes", fileName);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Resume uploaded successfully",
                ResumePath = user.ResumePath
            });
        }

        [HttpGet("download/{userId}")]
        [Authorize(Roles = "Recruiter,Admin")]
        public async Task<IActionResult> DownloadResume(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (string.IsNullOrEmpty(user.ResumePath))
            {
                return NotFound("Resume not found.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), user.ResumePath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Resume file not found.");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(
                fileBytes,
                "application/pdf",
                Path.GetFileName(filePath)
            );
        }
    }
}