using OpenLink.Models;
using Python.Runtime;
using System;
using System.IO;

namespace OpenLink.Services
{
    public class LLMService
    {
        private readonly ShortTermMemoryService shortTermMemoryService = new();
        private string chatHistory = "";
        //private string llm = "gemma2:27b";
        private static string llm = "llama3.1:8b";

       public LLMService()
        {
            Runtime.PythonDLL = "/opt/homebrew/opt/python@3.11/Frameworks/Python.framework/Versions/3.11/lib/libpython3.11.dylib";
            PythonEngine.Initialize();
        }

        public string LLM
        {
            get => llm;
            set => llm = value;
        }

        public string QueryLLM(string query)
        {
            Console.WriteLine("Query2:");
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(Directory.GetCurrentDirectory() + "/backend/Services");
                dynamic pythonScript = Py.Import("llmservice");

                shortTermMemoryService.CreateJson(query, "User");

                string path = Path.Combine(Directory.GetCurrentDirectory(), "backend", "Data", "ShortTermMemory.json");
                string json = File.ReadAllText(path);

                chatHistory = shortTermMemoryService.GetMaxTokens() + "User: " + "\n" + query;
                string result = pythonScript.askLLMAndGetResponse(chatHistory + "**DONT INCLUDE YOUR ANSWER WITH 'LLM(YOU):', AND NO NEED TO COMMENT ABOUT THE HISTORY OR THIS. IMPORTANT: JUST CONTINUE WITH YOUR ANSWER BASED ON THE HISTORY OF THIS CONVERSATION LIKE A USUAL CONVERSATION:**", LLM);

                shortTermMemoryService.CreateJson(result, "LLM(you)");
                chatHistory = shortTermMemoryService.GetMaxTokens();

                return result;
            }
        }

        public string GetChatHistory()
        {
            return chatHistory;
        }
    }
}