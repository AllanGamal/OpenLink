using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Python.Runtime;


namespace OpenLink.Services
{
    public class LLMService
    {

        private string? apiKey;
        private string modelId;
        private string endpoint;

        public LLMService(string? apiKey = null, string modelId = "phi3:medium-128k", string endpoint = "http://localhost:11434")
        {
            this.apiKey = apiKey;
            this.modelId = modelId;
            this.endpoint = endpoint;
        }

#pragma warning disable SKEXP0010 // Disable the warning for experimental API usage
        public Kernel InitializeKernel()
        {
            
            var kernelBuilder = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: modelId,
                apiKey: this.apiKey,
                endpoint: new Uri(this.endpoint)
                );
            var kernel = kernelBuilder.Build();
            return kernel;
        }

        public async Task<string> GetChatResponseAsync(string question, ChatHistory chatHistory)
        {
            Kernel kernel = InitializeKernel();
            var ai = kernel.GetRequiredService<IChatCompletionService>();

            chatHistory.AddUserMessage(question);
            StringBuilder stringBuilder = new StringBuilder();

            await foreach (var message in ai.GetStreamingChatMessageContentsAsync(chatHistory, kernel: kernel))
            {
                stringBuilder.Append(message.Content);
            }

            var fullResponse = stringBuilder.ToString();
            chatHistory.AddAssistantMessage(fullResponse);

            return fullResponse;
        }

        public async Task RunChatSessionAsync()
        {
            var question = "";

            while (question != "exit")
            {
                Console.WriteLine("Question: ");
            question = Console.ReadLine();
            Task<string> fullResponse = GetChatResponseAsync(question, new ChatHistory());
            Console.WriteLine("Response: ");
            Console.WriteLine(await fullResponse);
            Console.WriteLine();
               
            }
        }

        public static void GetStringFromPy()
        {
            dynamic script = Py.Import("OpenLink/backend/Services/llmservice");
            string response = script.example("Allan").ToString();
            Console.WriteLine(response);
            
        }


        public static void Main()
    {
        

            Runtime.PythonDLL = "/opt/homebrew/Cellar/python@3.8/3.8.19/Frameworks/Python.framework/Versions/3.8/lib/libpython3.8.dylib";

        
            PythonEngine.Initialize();
            using (Py.GIL())
        {
            /*await RunChatSessionAsync();*/
            GetStringFromPy();
        }

        }
    }
}
    