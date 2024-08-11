
from langchain_community.llms import Ollama
import sys, os
from ShortTermMemoryService import ShortTermMemoryService
from LongTermMemoryService import LongTermMemoryService

        

# class LLMService:

class LLMService:
    llm = "llama3.1:8b"
    short_term_memory_service = None
    long_term_memory_service = None

    def __init__(self, short_term_memory_service, long_term_memory_service):
        self.short_term_memory_service = short_term_memory_service
        self.long_term_memory_service = long_term_memory_service
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
    
    def improve_semantic_of_query(self, query: str) -> str:
        question = f'''
        input: {query}. 
        Take the user’s input before this sentence and rewrite it into a more detailed and semantically enriched query. 
        Focus on clarifying the intent and semantics, adding necessary context, and ensuring that the query is precise and comprehensive. 
        The goal is to capture the full meaning and nuances of the user’s original request to ensure the best possible response. 
        Keep in below 50 words. And do NOT type anything other than the rewritten query. And dont use "I" or "me" in the query, it should be general.
        '''
        result = self.askLLMAndGetResponse(question, self.LLM)
        return result
    

    
    def query_llm(self, question):
        self.short_term_memory_service.create_json(question, "User")

        path = "Data/ShortTermMemory.json" # from servers dir (backend folder)

        with open(path, 'r') as file:
            json_data = file.read()

        self.chat_history = self.short_term_memory_service.get_max_tokens() + "User: " + "\n" + question

        result = self.askLLMAndGetResponse(self.chat_history + "**DONT INCLUDE YOUR ANSWER WITH 'LLM(YOU):', AND NO NEED TO COMMENT ABOUT THE HISTORY OR THIS. IMPORTANT: JUST CONTINUE WITH YOUR ANSWER BASED ON THE HISTORY OF THIS CONVERSATION LIKE A USUAL CONVERSATION AND ANSWER THE QUESTION:**" + question, self.LLM)

        self.short_term_memory_service.create_json(result, "LLM(you)")
        self.chat_history = self.short_term_memory_service.get_max_tokens()

        

        return result

