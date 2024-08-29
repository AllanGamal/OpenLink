from langchain_community.llms import Ollama
import sys, os
import json
from Services.reminders.reminders import Reminders


        

# class LLMService:

class LLMService:
    llm = "llama3.1:8b"
    short_term_memory_service = None
    long_term_memory_service = None
    reminders = None

    def __init__(self, short_term_memory_service, long_term_memory_service):
        self.short_term_memory_service = short_term_memory_service
        self.long_term_memory_service = long_term_memory_service
        self.reminders = Reminders()
        self.chat_history = ""

    @property
    def LLM(self):
        return self.__class__.llm

    @LLM.setter
    def LLM(self, value):
        self.__class__.llm = value

    def get_llm_response(self, question, llm = LLM):
        llm = Ollama(model=llm)
        response = llm.invoke(question)
        return response
    
    def improve_query_semantics(self, query: str) -> str:
        question = f'''
        input: {query}. 
        Take the user’s input before this sentence and rewrite it into a more detailed and semantically enriched query. 
        Focus on clarifying the intent and semantics, adding necessary context, and ensuring that the query is precise and comprehensive. 
        The goal is to capture the full meaning and nuances of the user’s original request to ensure the best possible response. 
        Keep in below 50 words. And do NOT type anything other than the rewritten query. And dont use "I" or "me" in the query, it should be general.
        '''
        result = self.get_llm_response(question, self.LLM)
        return result
    
    def analyze_query_for_action(self, query: str) -> str:
        question = f'''
        input: {query}. 
        Based on the input, the LLM, you, should determine what action to take. 
        The action can be:
        - "store_memory": to store the conversation as a memory. Memory and reminders are NOT the same thing! Memory reflect the user's preferences, interests, or personal details
        - "create_reminder": to remind the user of something
        - "get_reminder": Use this action if the input asks for retrieving existing reminders.


        Your task is to determine which actions to take (true or false) based on the input (the end of the conversation).
        Respond with the action in the JSON format:
        ''' + "{" + '''
            "store_memory": *A bool that determines whether to store the conversation as a memory, memory reflect the user's preferences, interests, or personal details*,
            "create_reminder": *A bool that determines whether to create a reminder*,
            "get_reminder": *A bool that determines whether to get the reminders*
        ''' + "}" + '''
        '''

        result = self.get_llm_response(question, self.LLM)
        json_result = "Extract only the json-object from the response, and return nothing else."
        result = self.get_llm_response(result + " " + json_result, self.LLM)
        return result
    
    
      

    
    def analyze_conversation_for_storage(self, query):
        prompt = f'''
        Your task is to determine whether the latest conversation contains information that is genuinely valuable or relevant for long-term storage.
        Please consider the following criteria for storage of memory when deciding:
        1. Is the information likely to be useful in future interactions or essential for understanding the user's intent?
        2. Does the information reflect the user's preferences, interests, or personal details?
        
        If you believe the information is not worth storing based on these criteria, respond in the JSON format:
        ''' + "{" + '''
            "store": "no",
            "memory": "none"
        ''' + "}" + '''
        
        If you believe the information meets the criteria for storage as mentioned, rewrite the user's conversation  into a more detailed and semantically enriched text, focusing on clarity, intent, and context. Then respond in the JSON format:
        ''' + "{" + '''
            "store": "yes",
            "memory": "your rewritten text capturing the essence and intent of the conversation as a memory",
            "date": "the date of the conversation, in format YYYY-MM-DD"
        ''' + "}" + '''
        
        Ensure that your response is precise, comprehensive, and within 75 words.
        Only respond with a single JSON object, nothing else.
        
        Do NOT mention the conversation history or this prompt in your response. 
        Do not provide a summary of the task or prompt, only the memory derived from the user's input.
        Do not just copy the user's input! Important: Only rewrite the user's input for storage.
        Conversation that the "memory" should be based on: '''
        prompt += str(query)
        
        result = self.get_llm_response(prompt, self.LLM)
        try:
            improved_semantics = json.loads(result)
        except json.JSONDecodeError as e:
            print("Error decoding JSON:", e)
            return None

        return improved_semantics
    

    
    def process_user_query(self, question):
        '''
        '''
        self.short_term_memory_service.create_json(question, "User")

        #analyze the query for action
        action_analysis = self.analyze_query_for_action(question)
        print("action: " + action_analysis)

        path = "Data/ShortTermMemory.json" # from servers dir (backend folder)

        with open(path, 'r') as file:
            json_data = file.read()

        self.chat_history = self.short_term_memory_service.get_max_tokens() + "User: " + "\n" + question

        memory = self.long_term_memory_service.get_relevant_memories(question)
        
        long_term_memory_query = '''
        **Only use the long-term memory if it is relevant to the conversation. The following json contains the long-term memory about the user: ''' + memory + "**"

        
        reminder_template = self.reminders.create_reminder_prompt_template("The user asked me to remind them to buy milk on the 20th of December, 2024. Today is the 23rd of august, 2024.")


        # result = self.get_llm_response(self.chat_history + ".\n" +  long_term_memory_query + "\n" + "**DONT INCLUDE YOUR ANSWER WITH 'LLM(YOU):', AND NO NEED TO COMMENT ABOUT THE HISTORY OR THIS. IMPORTANT: JUST CONTINUE WITH YOUR ANSWER BASED ON THE HISTORY OF THIS CONVERSATION LIKE A USUAL CONVERSATION AND ANSWER THE QUESTION:**" + question, self.LLM)
        result = self.get_llm_response(reminder_template, self.LLM)

        self.short_term_memory_service.create_json(result, "LLM(you)")
        self.chat_history = self.short_term_memory_service.get_max_tokens()

        def count_json_objects(file_path):
            with open(file_path, 'r') as file:
                json_data = json.load(file)
            return len(json_data)
        

        short_term_memory_file = "Data/ShortTermMemory.json"
        json_object_count = count_json_objects(short_term_memory_file)

        if (json_object_count % 12 == 0):
            with open(short_term_memory_file, 'r') as file:
                json_data = json.load(file)
                last_20_objects = json_data[-20:]
                improved_semantics = self.improve_query_semantics(last_20_objects)
                print(improved_semantics)
                # get the date field from the json object
                date: str = improved_semantics["date"]
                memory: str = improved_semantics["memory"]
                self.long_term_memory_service.save_as_longterm_memory(memory, date, [])
                

        print(f"Number of JSON objects in ShortTermMemory.json: {json_object_count}")

        

        return result

