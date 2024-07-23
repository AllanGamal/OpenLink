using Microsoft.AspNetCore.Mvc;
using OpenLink.Services;

namespace OpenLink.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LLMController : ControllerBase
    {
        private readonly LLMService llmService;

        public LLMController()
        {
            llmService = new LLMService();
        }

        [HttpPost("query")]
        public IActionResult Query([FromBody] string query)
        {
            string response = llmService.QueryLLM(query);
            return Ok(response);
        }

        // This method can be used for console interaction, but typically,
        // web applications wouldn't use such methods.
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
                    string response = llmService.QueryLLM(query);
                    Console.WriteLine();
                    Console.WriteLine("Response:");
                    Console.WriteLine(response);
                }
            }
            Console.WriteLine(llmService.GetChatHistory());
        }

        // The main method can be used to run console mode for testing purposes.
        public static void Main()
        {
            LLMController controller = new LLMController();
            controller.StartConversationMode();
        }
    }
}