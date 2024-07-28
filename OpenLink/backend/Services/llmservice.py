
from langchain_community.llms import Ollama
import sys, os
        

# class LLMService:

class LLMService:
    llm = "llama3.1:8b"
    short_term_memory_service = None

    def __init__(self, short_term_memory_service):
        self.short_term_memory_service = short_term_memory_service
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
        self.short_term_memory_service.create_json(question, "User")

        path = "Data/ShortTermMemory.json" # from servers dir (backend folder)

        with open(path, 'r') as file:
            json_data = file.read()

        self.chat_history = self.short_term_memory_service.get_max_tokens() + "User: " + "\n" + question

        result = self.askLLMAndGetResponse(self.chat_history + "**DONT INCLUDE YOUR ANSWER WITH 'LLM(YOU):', AND NO NEED TO COMMENT ABOUT THE HISTORY OR THIS. IMPORTANT: JUST CONTINUE WITH YOUR ANSWER BASED ON THE HISTORY OF THIS CONVERSATION LIKE A USUAL CONVERSATION:**", self.LLM)

        self.short_term_memory_service.create_json(result, "LLM(you)")
        self.chat_history = self.short_term_memory_service.get_max_tokens()

        

        return result
    
        



