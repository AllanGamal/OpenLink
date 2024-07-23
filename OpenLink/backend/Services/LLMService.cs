using OpenLink.Models;
using Python.Runtime;
using System;
using System.IO;

namespace OpenLink.Services
{
    public class LLMService
    {
        private ShortTermMemoryModel ShortTermMemoryModel = new ShortTermMemoryModel();
        private string chatHistory = "";
        private string llm = "gemma2:27b";
        //private static string llm = "llama3:8b";

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
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(Directory.GetCurrentDirectory() + "/backend/Services");
                dynamic pythonScript = Py.Import("LLMService");

                ShortTermMemoryModel.CreateJson(query, "User");

                string path = Path.Combine(Directory.GetCurrentDirectory(), "backend", "Data", "ShortTermMemory.json");
                string json = File.ReadAllText(path);

                chatHistory = ShortTermMemoryModel.GetMaxTokens() + "User: " + "\n" + query + "**YOUR ANSWER BASED ON THE HISTORY:**";
                string result = pythonScript.askLLMAndGetResponse(chatHistory, LLM);

                ShortTermMemoryModel.CreateJson(result, "LLM(you)");
                chatHistory = ShortTermMemoryModel.GetMaxTokens();

                return result;
            }
        }

        public string GetChatHistory()
        {
            return chatHistory;
        }
    }
}