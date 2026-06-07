using BCrypt.Net;
using ICMS.API.DTOs;
using ICMS.API.Services;
using ICMS.Data.Context;
using ICMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ICMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuditLogService _auditLogService;

        public AuthController
        (
            ApplicationDbContext context,
            IConfiguration configuration,
            AuditLogService auditLogService
        )
        {
            _context = context;
            _configuration = configuration;
            _auditLogService = auditLogService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }

            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            await _auditLogService.LogAsync(
                user.Id,
                user.Email ?? "",
                "REGISTER",
                "User",
                user.Id,
                $"New user registered with role {user.Role}"
            );

            return Ok(new
            {
                message = "User registered successfully"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.PasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password");
            }

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email!
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.Role!
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.FullName!
                )
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            await _auditLogService.LogAsync(
                user.Id,
                user.Email ?? "",
                "LOGIN",
                "User",
                user.Id,
                $"{user.FullName} logged in"
            );

            return Ok(new
            {
                token = jwtToken
            });
        }
    }
}