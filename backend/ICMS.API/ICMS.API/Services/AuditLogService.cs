using ICMS.Data.Context;
using ICMS.Models;

namespace ICMS.API.Services
{
    public class AuditLogService
    {
        private readonly ApplicationDbContext _context;

        public AuditLogService(
            ApplicationDbContext context
        )
        {
            _context = context;
        }
        public async Task LogAsync(
            int? userId,
            string userEmail,
            string action,
            string entityType,
            int? entityId,
            string description)
        {
            var log = new AuditLog
            {
                UserId = userId,
                UserEmail = userEmail,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };
            await _context.AuditLogs.AddAsync(log);

            await _context.SaveChangesAsync();
        }

        public async Task LogJobAsync(
            Job job,
            string recruiterEmail,
            string action)
        {
            var log = new JobAudit
            {
                JobId = job.Id,
                Title = job.Title ?? string.Empty,
                CompanyName = job.CompanyName ?? string.Empty,
                Location = job.Location ?? string.Empty,
                RecruiterId = job.RecruiterId,
                RecruiterEmail = recruiterEmail,
                PostedDate = job.PostedDate,
                Action = action,
                CreatedAt = DateTime.UtcNow
            };
            await _context.JobAudits.AddAsync(log);

            await _context.SaveChangesAsync();
        }
    }
}