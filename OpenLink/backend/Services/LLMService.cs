using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.VisualBasic;

namespace OpenLink.Services
{
    public class LLMService
    {

#pragma warning disable SKEXP0010 // Disable the warning for experimental API usage
        public Kernel InitializeKernel()
        {
            var kernelBuilder = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: "phi3:medium-128k",
                apiKey: null,
                endpoint: new Uri("http://localhost:11434")
                );
            var kernel = kernelBuilder.Build();
            return kernel;
        }


        public async void main()
        {
            Kernel k = InitializeKernel();
            var ai = k.GetRequiredService<IChatCompletionService>();
            ChatHistory chatHistory = new ("You are an AI assistant that helps me with my work.");
            StringBuilder sb = new();
            
            Console.WriteLine("Question: ");
            var question = Console.ReadLine();

            while (question != "exit")
            {
                chatHistory.AddUserMessage(question);
                sb.Clear();

                await foreach (var message in ai.GetStreamingChatMessageContentsAsync(chatHistory, kernel: k)) 
                {
                    Console.Write(message);
                    sb.AppendLine(message.Content);
                }
            Console.WriteLine();
            Console.WriteLine();
            chatHistory.AddAssistantMessage(sb.ToString());
            Console.WriteLine("Question: ");
            question = Console.ReadLine();
            }

            
    }
}
}