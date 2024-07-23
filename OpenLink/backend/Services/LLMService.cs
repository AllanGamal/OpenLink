using OpenLink.Models;
using Python.Runtime;


namespace OpenLink.Services
{

    public class LLMService
    {

        private static ShortTermMemoryModel ShortTermMemoryModel = new ShortTermMemoryModel(); 
        private static string chatHistory = "";
        private static string llm = "gemma2:27b";
        //private static string llm = "llama3:8b";

        // get set

        // get and set
        static LLMService()
        {
            Runtime.PythonDLL = "/opt/homebrew/opt/python@3.11/Frameworks/Python.framework/Versions/3.11/lib/libpython3.11.dylib";
            PythonEngine.Initialize();
        }
        public static string LLM
        {
            get => llm;
            set => llm = value;
        }
        

        public static void QueryLLM(string query)
        {
            
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(Directory.GetCurrentDirectory() + "/backend/Services");
                dynamic pythonScript = Py.Import("LLMService");
                ShortTermMemoryModel.CreateJson(query , "User");
                
                
                string path = Path.Combine(Directory.GetCurrentDirectory(), "backend", "Data", "ShortTermMemory.json");
                string json = File.ReadAllText(path);

                chatHistory = ShortTermMemoryModel.GetMaxTokens() +  "User: " + "\n" + query + "**YOUR ANSWER BASED ON THE HISTORY:**";

                string result = pythonScript.askLLMAndGetResponse(chatHistory, LLM);

                ShortTermMemoryModel.CreateJson(result, "LLM(you)");
                chatHistory = ShortTermMemoryModel.GetMaxTokens();
                Console.WriteLine();
                Console.WriteLine("Response:");
                Console.WriteLine(result);

            }
        }
        

        private static void ConversationMode()
        {
            
                // add query from terminal
                string? query = "";
                while (query != "exit")
                {
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine("Query:");
                    query = Console.ReadLine();
                    QueryLLM(query);
                    
                }
                Console.WriteLine(chatHistory);

            
        }
            public static void Main()
            {
                
                ConversationMode();
            }
    
}
}