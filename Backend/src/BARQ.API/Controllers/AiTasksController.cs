using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace BARQ.API.Controllers
{
    [ApiController]
    [Route("api/ai-tasks")]
    [Authorize]
    public class AiTasksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(new object[0]);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetTask(Guid id)
        {
            return Ok(new { id });
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] object request)
        {
            var id = Guid.NewGuid();
            return CreatedAtAction(nameof(GetTask), new { id = id }, new { id });
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteTask(Guid id)
        {
            return NoContent();
        }
    }
}
