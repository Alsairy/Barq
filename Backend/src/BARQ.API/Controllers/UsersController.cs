using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BARQ.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private static readonly List<UserSummaryDto> Users = new List<UserSummaryDto>
        {
            new UserSummaryDto
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Email = "test@acme.com",
                FirstName = "Acme",
                PhoneNumber = "1234567890",
                LastName = "User"
            },
            new UserSummaryDto
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                
                OrganizationId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Email = "test@beta.com",
                FirstName = "Jane",
                PhoneNumber = "0987654321",
                LastName = "Smith"
            }
        };

        private UserSummaryDto GetCurrentUser()
        {
                
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (Guid.TryParse(userIdStr, out var userId))
            {
                return Users.FirstOrDefault(u => u.Id == userId);
            }
            return Users.First();
        }

        [HttpGet("profile")]
        public ActionResult<UserSummaryDto> GetProfile()
        {
            var user = GetCurrentUser();
            if (user == null) return Unauthorized();
            return Ok(user);
        }

        [HttpPost("profile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileRequestDto request)
        {
            var user = GetCurrentUser();
            if (user == null) return Unauthorized();
            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;
            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;
            
                        if (!string.IsNullOrEmpty(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;
            return Ok(user);
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserSummaryDto>> GetUsers()
        {
            var user = GetCurrentUser();
            if (user == null) return Unauthorized();
            var list = Users.Where(u => u.OrganizationId == user.OrganizationId).ToList();
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<UserSummaryDto> GetUser(Guid id)
        {
            var current = GetCurrentUser();
            if (current == null) return Unauthorized();
            var user = Users.FirstOrDefault(u => u.Id == id && u.OrganizationId == current.OrganizationId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        public class CreateUserRequestDto
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public ActionResult<UserSummaryDto> CreateUser([FromBody] CreateUserRequestDto request)
        {
            var current = GetCurrentUser();
            if (current == null) return Unauthorized();
            if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
                return BadRequest();
            var newUser = new UserSummaryDto
            {
                Id = Guid.NewGuid(),
                OrganizationId = current.OrganizationId,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            Users.Add(newUser);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            var current = GetCurrentUser();
            if (current == null) return Unauthorized();
            return Ok();
        }

        public class UpdateProfileRequestDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
        }

        public class ChangePasswordRequestDto
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string CurrentPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public class UserSummaryDto
        {
            public Guid Id { get; set; }
            public Guid OrganizationId { get; set; }
                    public string PhoneNumber { get; set; }

            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
