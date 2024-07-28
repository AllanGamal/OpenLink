using Microsoft.AspNetCore.Mvc;
using OpenLink.Services;

namespace OpenLink.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LLMController : ControllerBase
    {
        private readonly LLMService _llmService;

        public LLMController(LLMService llmService)
        {
            _llmService = llmService;
        }

        public class QueryRequest
        {
            public string Query { get; set; }
        }

        [HttpPost("query")]
        public IActionResult Query([FromBody] QueryRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Query))
            {
                return BadRequest("Query is required");
            }

            string response = _llmService.QueryLLM(request.Query);
            return Ok(response);
        }

        public void StartConversationMode()
        {
            string? query = "";
            while (query != "exit")
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Query:");
                query = Console.ReadLine();
                if (query != "exit")
                {
                    string response = _llmService.QueryLLM(query);
                    Console.WriteLine();
                    Console.WriteLine("Response:");
                    Console.WriteLine(response);
                }
            }
            Console.WriteLine(_llmService.GetChatHistory());
        }

        public static void Main()
        {
           // LLMController controller = new LLMController();
           // controller.StartConversationMode();
        }
    }
}