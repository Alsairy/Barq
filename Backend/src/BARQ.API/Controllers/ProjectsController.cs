using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BARQ.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private static readonly List<UserSummaryDto> Users = new List<UserSummaryDto>
        {
            new UserSummaryDto { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            new UserSummaryDto { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), OrganizationId = Guid.Parse("22222222-2222-2222-2222-222222222222") }
        };

        private static readonly List<ProjectSummaryDto> Projects = new List<ProjectSummaryDto>
        {
            new ProjectSummaryDto
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Acme Project"
            },
            new ProjectSummaryDto
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                OrganizationId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Beta Project"
            }
        };

        private Guid GetCurrentOrganizationId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (Guid.TryParse(userIdStr, out var userId))
            {
                var user = Users.FirstOrDefault(u => u.Id == userId);
                if (user != null) return user.OrganizationId;
            }
            return Users.First().OrganizationId;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProjectSummaryDto>> GetProjects()
        {
            var orgId = GetCurrentOrganizationId();
            var list = Projects.Where(p => p.OrganizationId == orgId).ToList();
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<ProjectSummaryDto> GetProject(Guid id)
        {
            var orgId = GetCurrentOrganizationId();
            var project = Projects.FirstOrDefault(p => p.Id == id && p.OrganizationId == orgId);
            if (project == null) return NotFound();
            return Ok(project);
        }

        public class CreateProjectRequestDto
        {
            public string Name { get; set; }
        }

        [HttpPost]
        public ActionResult<ProjectSummaryDto> CreateProject([FromBody] CreateProjectRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest();
            var orgId = GetCurrentOrganizationId();
            var project = new ProjectSummaryDto
            {
                Id = Guid.NewGuid(),
                OrganizationId = orgId,
                Name = request.Name
            };
            Projects.Add(project);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        public class UpdateProjectRequestDto
        {
            public string Name { get; set; }
        }

        [HttpPost("{id:guid}")]
        public IActionResult UpdateProject(Guid id, [FromBody] UpdateProjectRequestDto request)
        {
            var orgId = GetCurrentOrganizationId();
            var project = Projects.FirstOrDefault(p => p.Id == id && p.OrganizationId == orgId);
            if (project == null) return NotFound();
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest();
            project.Name = request.Name;
            return Ok(project);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProject(Guid id)
        {
            var orgId = GetCurrentOrganizationId();
            var project = Projects.FirstOrDefault(p => p.Id == id && p.OrganizationId == orgId);
            if (project == null) return NotFound();
            Projects.Remove(project);
            return NoContent();
        }

        public class UserSummaryDto
        {
            public Guid Id { get; set; }
            public Guid OrganizationId { get; set; }
        }

        public class ProjectSummaryDto
        {
            public Guid Id { get; set; }
            public Guid OrganizationId { get; set; }
            public string Name { get; set; }
        }
    }
}
