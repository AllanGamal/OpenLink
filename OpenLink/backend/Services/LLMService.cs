using Python.Runtime;


namespace OpenLink.Services
{
    public class LLMService
    {

        private string modelId;
        private static string chatHistory;
        private static string llm = "llama3:8b";

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
                chatHistory += "User: " + query + "\n";

                string result = pythonScript.askLLMAndGetResponse(chatHistory, LLM);

                chatHistory += "LLM: " + result + "\n";
                Console.WriteLine();
                Console.WriteLine("Response: ");
                Console.WriteLine(result);


            }

        }

        // get set llm
        

        private static void ConversationMode()
        {
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
        }
            public static void Main()
            {
                LLM = "phi3:medium-128k";
                ConversationMode();
            }
    
}
}