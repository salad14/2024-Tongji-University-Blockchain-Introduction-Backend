using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCompetitionAPI.Data;
using StudentCompetitionAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.Web3;

namespace StudentCompetitionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionAwardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompetitionAwardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CompetitionAwards?studentId=1
        [HttpGet]
        public async Task<IActionResult> GetAwards([FromQuery] int studentId)
        {
            var awards = await _context.CompetitionAwards
                .Where(ca => ca.StudentId == studentId)
                .ToListAsync();

            return Ok(awards);
        }

        // POST: api/CompetitionAwards
        [HttpPost]
        public async Task<IActionResult> SubmitAward([FromBody] SubmitAwardRequest request)
        {
            // 验证学生是否存在
            var studentExists = await _context.Users.AnyAsync(u => u.Id == request.StudentId);
            if (!studentExists)
            {
                return BadRequest("学生ID不存在");
            }

            var award = new CompetitionAward
            {
                StudentId = request.StudentId,
                CompetitionName = request.CompetitionName,
                AwardLevel = request.AwardLevel,
                AwardDate = request.AwardDate,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CompetitionAwards.Add(award);
            await _context.SaveChangesAsync();

            return Ok("提交成功，待审核");
        }

        // PUT: api/CompetitionAwards/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAward(int id, [FromBody] UpdateAwardRequest request)
        {
            var award = await _context.CompetitionAwards.FindAsync(id);

            if (award == null)
            {
                return NotFound("获奖信息未找到");
            }

            award.CompetitionName = request.CompetitionName ?? award.CompetitionName;
            award.AwardLevel = request.AwardLevel ?? award.AwardLevel;
            award.AwardDate = request.AwardDate ?? award.AwardDate;
            award.UpdatedAt = DateTime.UtcNow;

            _context.CompetitionAwards.Update(award);
            await _context.SaveChangesAsync();

            return Ok("获奖信息更新成功");
        }

        // POST: api/CompetitionAwards/Audit/{id}
        [HttpPost("Audit/{id}")]
        public async Task<IActionResult> AuditAward(int id, [FromBody] AuditRequest request)
        {
            var award = await _context.CompetitionAwards.FindAsync(id);

            if (award == null)
            {
                return NotFound("获奖信息未找到");
            }

            // 仅允许从 "Pending" 状态进行审核
            if (award.Status != "Pending")
            {
                return BadRequest("只有待审核的获奖信息可以进行审核");
            }

            award.Status = request.Status;
            award.UpdatedAt = DateTime.UtcNow;

            _context.CompetitionAwards.Update(award);
            await _context.SaveChangesAsync();

            return Ok($"获奖信息审核状态更新为 {request.Status}");
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllAwardsSimple()
        {
            var awards = await _context.CompetitionAwards
                .Select(ca => new
                {
                    ca.Id,
                    ca.StudentId,
                    ca.CompetitionName,
                    ca.AwardLevel,
                    ca.AwardDate,
                    ca.Status,
                    ca.CreatedAt,
                    ca.UpdatedAt
                })
                .ToListAsync();

            return Ok(awards);
        }
    }

    // 请求模型
    public class SubmitAwardRequest
    {
        public int StudentId { get; set; }
        public string CompetitionName { get; set; }
        public string AwardLevel { get; set; }
        public DateTime AwardDate { get; set; }
    }

    public class UpdateAwardRequest
    {
        public string CompetitionName { get; set; }
        public string AwardLevel { get; set; }
        public DateTime? AwardDate { get; set; }
    }

    public class AuditRequest
    {
        public string Status { get; set; } // 如："Approved" 或 "Rejected"
    }
}
