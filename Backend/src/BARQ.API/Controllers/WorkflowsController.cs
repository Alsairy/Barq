using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace BARQ.API.Controllers
{
    [ApiController]
    [Route("api/workflows")]
    [Authorize]
    public class WorkflowsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWorkflows()
        {
            return Ok(new object[0]);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetWorkflow(Guid id)
        {
            return Ok(new { id });
        }

        [HttpPost]
        public IActionResult CreateWorkflow([FromBody] object request)
        {
            var id = Guid.NewGuid();
            return CreatedAtAction(nameof(GetWorkflow), new { id = id }, new { id });
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteWorkflow(Guid id)
        {
            return NoContent();
        }
    }
}
