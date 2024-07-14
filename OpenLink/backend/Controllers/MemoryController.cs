using Microsoft.AspNetCore.Mvc;

namespace OpenLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemoryController : ControllerBase
    {
        private readonly MemoryService _memoryService;

        public MemoryController(MemoryService memoryService)
        {
            _memoryService = memoryService;
        }

        [HttpGet]
        public IActionResult GetAllMemories()
        {
            var memories = _memoryService.GetAllMemories();
            return Ok(memories);
        }

        [HttpGet("{id}")]
        public IActionResult GetMemoryById(int id)
        {
            var memory = _memoryService.GetMemoryById(id);
            if (memory == null)
            {
                return NotFound();
            }
            return Ok(memory);
        }

        [HttpPost]
        public IActionResult CreateMemory(MemoryModel memory)
        {
            _memoryService.CreateMemory(memory);
            return CreatedAtAction(nameof(GetMemoryById), new { id = memory.Id }, memory);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMemory(int id, MemoryModel memory)
        {
            if (id != memory.Id)
            {
                return BadRequest();
            }

            var existingMemory = _memoryService.GetMemoryById(id);
            if (existingMemory == null)
            {
                return NotFound();
            }

            _memoryService.UpdateMemory(memory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMemory(int id)
        {
            var memory = _memoryService.GetMemoryById(id);
            if (memory == null)
            {
                return NotFound();
            }

            _memoryService.DeleteMemory(id);
            return NoContent();
        }
    }
}

