using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using BARQ.Infrastructure.Data;
using BARQ.Core.Entities;

namespace BARQ.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly BarqDbContext _db;

        public UsersController(BarqDbContext db)
        {
            _db = db;
        }

        private (Guid TenantId, Guid UserId, string Email) GetContextFromClaims()
        {
            var tenantIdStr = User.FindFirstValue("tenant_id") ?? Guid.Empty.ToString();
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? Guid.Empty.ToString();
            var email = User.FindFirstValue(ClaimTypes.Email)
                        ?? User.FindFirstValue("email")
                        ?? User.FindFirstValue("preferred_username")
                        ?? User.FindFirstValue(ClaimTypes.Name)
                        ?? "user@example.com";
            Guid.TryParse(tenantIdStr, out var tenantId);
            Guid.TryParse(userIdStr, out var userId);

            if (tenantId == Guid.Empty)
            {
                var tp = HttpContext.RequestServices.GetService<BARQ.Core.Services.ITenantProvider>();
                if (tp != null)
                {
                    var fallbackTid = tp.GetTenantId();
                    if (fallbackTid != Guid.Empty)
                    {
                        tenantId = fallbackTid;
                    }
                }
            }

            return (tenantId, userId, email);
        }

        [HttpGet("profile")]
        public ActionResult<object> GetProfile()
        {
            var ctx = GetContextFromClaims();
            var user = _db.Users.FirstOrDefault(u => u.TenantId == ctx.TenantId && u.Id == ctx.UserId)
                ?? _db.Users.FirstOrDefault(u => u.TenantId == ctx.TenantId && u.Email == ctx.Email);
            if (user == null) return Unauthorized();
            return Ok(new { id = user.Id, organizationId = user.TenantId, email = user.Email, firstName = user.FirstName, lastName = user.LastName });
        }

        [HttpPost("profile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var ctx = GetContextFromClaims();
            var user = _db.Users.FirstOrDefault(u => u.TenantId == ctx.TenantId && u.Id == ctx.UserId)
                ?? _db.Users.FirstOrDefault(u => u.TenantId == ctx.TenantId && u.Email == ctx.Email);
            if (user == null) return Unauthorized();
            if (!string.IsNullOrEmpty(request.FirstName)) user.FirstName = request.FirstName;
            if (!string.IsNullOrEmpty(request.LastName)) user.LastName = request.LastName;
            user.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();
            return Ok(new { id = user.Id, organizationId = user.TenantId, email = user.Email, firstName = user.FirstName, lastName = user.LastName });
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetUsers()
        {
            var ctx = GetContextFromClaims();
            var list = _db.Users
                .Where(u => u.TenantId == ctx.TenantId)
                .Select(u => new { id = u.Id, organizationId = u.TenantId, email = u.Email, firstName = u.FirstName, lastName = u.LastName })
                .ToList();
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<object> GetUser(Guid id)
        {
            var ctx = GetContextFromClaims();
            var user = _db.Users
                .Where(u => u.TenantId == ctx.TenantId && u.Id == id)
                .Select(u => new { id = u.Id, organizationId = u.TenantId, email = u.Email, firstName = u.FirstName, lastName = u.LastName })
                .FirstOrDefault();
            if (user == null) return NotFound();
            return Ok(user);
        }

        public class UpdateProfileRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
        }

        public class CreateUserRequest
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public ActionResult<object> CreateUser([FromBody] CreateUserRequest request)
        {
            var ctx = GetContextFromClaims();
            if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
                return BadRequest();
            var exists = _db.Users.Any(u => u.TenantId == ctx.TenantId && u.Email == request.Email);
            if (exists) return BadRequest();
            var entity = new User
            {
                Id = Guid.NewGuid(),
                TenantId = ctx.TenantId,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = "",
                Status = BARQ.Core.Enums.UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            _db.Users.Add(entity);
            _db.SaveChanges();
            return CreatedAtAction(nameof(GetUser), new { id = entity.Id }, new { id = entity.Id, organizationId = entity.TenantId, email = entity.Email, firstName = entity.FirstName, lastName = entity.LastName });
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var ctx = GetContextFromClaims();
            var user = _db.Users.FirstOrDefault(u => u.TenantId == ctx.TenantId && u.Id == ctx.UserId)
                ?? _db.Users.FirstOrDefault(u => u.TenantId == ctx.TenantId && u.Email == ctx.Email);
            if (user == null) return Unauthorized();
            return Ok();
        }

        public class ChangePasswordRequest
        {
            public string? OldPassword { get; set; }
            public string? CurrentPassword { get; set; }
            public string? NewPassword { get; set; }
            public string? ConfirmPassword { get; set; }
        }
    }
}
