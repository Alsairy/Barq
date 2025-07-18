using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BARQ.API.Controllers
{
    [ApiController]
    [Route("api/organizations")]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {
        private static readonly List<UserDto> Users = new List<UserDto>
        {
            new UserDto
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            },
            new UserDto
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                OrganizationId = Guid.Parse("22222222-2222-2222-2222-222222222222")
            }
        };

        private static readonly List<OrganizationDto> Organizations = new List<OrganizationDto>
        {
            new OrganizationDto
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Acme Corporation"
            },
            new OrganizationDto
            {
                Id = Guid "Beta Industries",
            }
        };

        private Guid GetCurrentOrganizationId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (Guid.TryParse(userIdStr, out var userId))
            {
                var user = Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                    return user.OrganizationId;
            }
            return Users.First().OrganizationId;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrganizationDto>> GetOrganizations()
        {
            var orgId = GetCurrentOrganizationId();
            var list = Organizations.Where(o => o.Id == orgId).ToList();
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<OrganizationDto> GetOrganization(Guid id)
        {
            var orgId = GetCurrentOrganizationId();
            var org = Organizations.FirstOrDefault(o => o.Id == id && o.Id == orgId);
            if (org == null)
                return NotFound();
            return Ok(org);
        }

        public class CreateOrganizationRequest
        {
            public string Name { get; set; }
        }

        [HttpPost]
        public ActionResult<OrganizationDto> CreateOrganization([FromBody] CreateOrganizationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest();
            var orgId = GetCurrentOrganizationId();
            var newOrg = new OrganizationDto
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };
            Organizations.Add(newOrg);
            return CreatedAtAction(nameof(GetOrganization), new { id = newOrg.Id }, newOrg);
        }

        public class UpdateOrganizationRequest
        {
            public string Name { get; set; }
        }

        [HttpPost("{id:guid}")]
        public IActionResult UpdateOrganization(Guid id, [FromBody] UpdateOrganizationRequest request)
        {
            var orgId = GetCurrentOrganizationId();
            var org = Organizations.FirstOrDefault(o => o.Id == id && o.Id == orgId);
            if (org == null)
                return NotFound();
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest();
            org.Name = request.Name;
            return Ok(org);
        }

        public class UserDto
        {
            public Guid Id { get; set; }
            public Guid OrganizationId { get; set; }
        }

        public class OrganizationDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
