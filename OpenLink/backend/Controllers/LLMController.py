class LLMController:

    llm_service = None

    def __init__(self, llm_service):
        self.llm_service = llm_service

    def start_conversation_mode(self, question):
        print("--------------------------------------")
        print("Query:")
        print(question)
      
        response = self.llm_service.query_llm(question)
        print("")
        print("Response:")
        print(response)
        return response