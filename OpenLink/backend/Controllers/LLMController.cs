using Microsoft.AspNetCore.Mvc;

namespace OpenLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LLMController : ControllerBase
    {
        private readonly LLMService _llmService;

        public LLMController(LLMService llmService)
        {
            _llmService = llmService;
        }

        [HttpGet]
        public IActionResult GetAllLLMs()
        {
            var llms = _llmService.GetAllLLMs();
            return Ok(llms);
        }

        [HttpGet("{id}")]
        public IActionResult GetLLMById(int id)
        {
            var llm = _llmService.GetLLMById(id);
            if (llm == null)
            {
                return NotFound();
            }
            return Ok(llm);
        }

        [HttpPost]
        public IActionResult CreateLLM(LLMModel llm)
        {
            _llmService.CreateLLM(llm);
            return CreatedAtAction(nameof(GetLLMById), new { id = llm.Id }, llm);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLLM(int id, LLMModel llm)
        {
            if (id != llm.Id)
            {
                return BadRequest();
            }

            var existingLLM = _llmService.GetLLMById(id);
            if (existingLLM == null)
            {
                return NotFound();
            }

            _llmService.UpdateLLM(llm);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLLM(int id)
        {
            var llm = _llmService.GetLLMById(id);
            if (llm == null)
            {
                return NotFound();
            }

            _llmService.DeleteLLM(id);
            return NoContent();
        }
    }
}

