using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BARQ.Infrastructure.Data;

namespace BARQ.API.Controllers
{
    [ApiController]
    [Route("api/organizations")]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {
        private readonly BarqDbContext _db;

        public OrganizationsController(BarqDbContext db)
        {
            _db = db;
        }

        private Guid GetTenantId()
        {
            var tenantIdStr = User.FindFirstValue("tenant_id");
            if (Guid.TryParse(tenantIdStr, out var tid) && tid != Guid.Empty)
                return tid;

            string? token = null;
            var auth = Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrWhiteSpace(auth) && auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = auth.Substring("Bearer ".Length).Trim();
            }
            else if (Request.Cookies.TryGetValue("__Host-Auth", out var cookieToken) && !string.IsNullOrWhiteSpace(cookieToken))
            {
                token = cookieToken;
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(token);
                    var tenantClaim = jwt.Claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
                    if (!string.IsNullOrWhiteSpace(tenantClaim) && Guid.TryParse(tenantClaim, out var tidFromToken) && tidFromToken != Guid.Empty)
                    {
                        return tidFromToken;
                    }
                }
                catch
                {
                }
            }

            var tp = HttpContext.RequestServices.GetService<BARQ.Core.Services.ITenantProvider>();
            if (tp != null)
            {
                var fallback = tp.GetTenantId();
                if (fallback != Guid.Empty) return fallback;
            }

            return Guid.Empty;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetOrganizations()
        {
            var tenantId = GetTenantId();

            var query = _db.Organizations.AsQueryable();
            if (tenantId != Guid.Empty)
            {
                query = query.Where(o => o.Id == tenantId);
            }

            var list = query
                .Select(o => new { id = o.Id, name = o.Name })
                .ToList();

            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<object> GetOrganization(Guid id)
        {
            var tenantId = GetTenantId();

            var org = _db.Organizations.FirstOrDefault(o => o.Id == id);
            if (org == null) return NotFound();
            if (tenantId != Guid.Empty && org.Id != tenantId) return NotFound();

            return Ok(new { id = org.Id, name = org.Name });
        }

        public class CreateOrganizationRequest
        {
            public string Name { get; set; }
        }

        [HttpPost]
        public ActionResult<object> CreateOrganization([FromBody] CreateOrganizationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest();

            var entity = new BARQ.Core.Entities.Organization
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Domain = "",
                SubscriptionPlan = BARQ.Core.Enums.SubscriptionPlan.Professional,
                Status = BARQ.Core.Enums.OrganizationStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            _db.Organizations.Add(entity);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetOrganization), new { id = entity.Id }, new { id = entity.Id, name = entity.Name });
        }

        public class UpdateOrganizationRequest
        {
            public string Name { get; set; }
        }

        [HttpPost("{id:guid}")]
        public IActionResult UpdateOrganization(Guid id, [FromBody] UpdateOrganizationRequest request)
        {
            var tenantId = GetTenantId();
            var org = _db.Organizations.FirstOrDefault(o => o.Id == id);
            if (org == null) return NotFound();
            if (tenantId != Guid.Empty && org.Id != tenantId) return NotFound();

            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest();

            org.Name = request.Name;
            org.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();

            return Ok(new { id = org.Id, name = org.Name });
        }
    }
}
