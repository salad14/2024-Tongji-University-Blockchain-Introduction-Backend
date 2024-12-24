using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCompetitionAPI.Data;
using StudentCompetitionAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentCompetitionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("用户名已存在");
            }

            var user = new User
            {
                Username = request.Username,
                Password = request.Password, // 建议在生产环境中对密码进行哈希处理
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("注册成功");
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("用户名或密码不正确");
            }

            // 返回用户信息（不包括密码）
            var userInfo = new
            {
                user.Id,
                user.Username,
                user.Email,
                user.CreatedAt,
                user.UpdatedAt
            };

            return Ok(userInfo);
        }

        // PUT: api/Auth/Update/{id}
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("用户未找到");
            }

            user.Username = request.Username ?? user.Username;
            user.Password = request.Password ?? user.Password; // 建议在生产环境中对密码进行哈希处理
            user.Email = request.Email ?? user.Email;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("用户信息更新成功");
        }
    }

    // 请求模型
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; } 
        public string Email { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; } 
    }

    public class UpdateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
