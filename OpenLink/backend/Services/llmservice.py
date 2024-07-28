
from langchain_community.llms import Ollama

from ShortTermMemoryService import ShortTermMemoryService
import sys, os
        

# class LLMService:

class LLMService:
    llm = "llama3.1:8b"

    def __init__(self):
        self.short_term_memory_service = ShortTermMemoryService()
        self.chat_history = ""

    @property
    def LLM(self):
        return self.__class__.llm

    @LLM.setter
    def LLM(self, value):
        self.__class__.llm = value

    def askLLMAndGetResponse(self, question, llm):
        llm = Ollama(model=llm)
        response = llm.invoke(question)
        return response
    
    def query_llm(self, question):
        sys.path.append(os.path.join(os.getcwd(), "backend", "Services"))

        self.short_term_memory_service.create_json(question, "User")

        path = os.path.join(os.getcwd(), "backend", "Data", "ShortTermMemory.json")
        path = "../Data/ShortTermMemory.json"

        with open(path, 'r') as file:
            json_data = file.read()

        self.chat_history = self.short_term_memory_service.get_max_tokens() + "User: " + "\n" + question

        result = self.askLLMAndGetResponse(self.chat_history + "**DONT INCLUDE YOUR ANSWER WITH 'LLM(YOU):', AND NO NEED TO COMMENT ABOUT THE HISTORY OR THIS. IMPORTANT: JUST CONTINUE WITH YOUR ANSWER BASED ON THE HISTORY OF THIS CONVERSATION LIKE A USUAL CONVERSATION:**", self.LLM)

        self.short_term_memory_service.create_json(result, "LLM(you)")
        self.chat_history = self.short_term_memory_service.get_max_tokens()

        print("-----------------")
        print("Chat history:" + self.chat_history)
        print("-----------------")

        return result
    
        

service = LLMService()
string = service.query_llm("Did we play any game? If so, what did we play?")
print(string)


