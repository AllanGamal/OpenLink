from langchain_chroma import Chroma
from langchain.docstore.document import Document
from langchain_huggingface import HuggingFaceEmbeddings
import json



class LongTermMemoryService:

    vector_directory = "Services/Data/VectorStore"
    embedding_function = HuggingFaceEmbeddings(model_name="intfloat/multilingual-e5-large")

    def __init__(self):
        pass

    
    def save_as_longterm_memory(self, memory: str, timestamp: str, list_of_tags: list[str]) -> None:

        documents = []
        if list_of_tags:
            documents = [Document(page_content=memory, metadata={"timestamp": timestamp, "tag1": list_of_tags[0], "tag2": list_of_tags[1], "tag3": list_of_tags[2]})]
        else:
            documents = [Document(page_content=memory, metadata={"timestamp": timestamp})]

        Chroma.from_documents(
        documents,
        self.embedding_function,
        persist_directory=self.vector_directory, # save in chromadb folder
    )
        
        
    def get_relevant_memories(self, query: str, list_of_tags: list[str] = None):
        database = Chroma(persist_directory=self.vector_directory, embedding_function=self.embedding_function) # load from the saved folder

        filter_condition = {}

        if (list_of_tags):
            filter_condition = filter_condition = {
            "$or": [
                {"tag1": {"$in": list_of_tags}},
                {"tag2": {"$in": list_of_tags}},
                {"tag3": {"$in": list_of_tags}}
            ]
        }

        relevant_documents = database.similarity_search(
            query=query,
            k=5,
             filter=filter_condition
            ) 
        
        # loop through the relevant documents and return the page content for every memory in a json format. memory1: ...
        memories = {}
        for i, doc in enumerate(relevant_documents):
            memory_key = f"memory{i+1}"
            date_only = doc.metadata["timestamp"].split("T")[0]
            memories[memory_key] = {
                "content": doc.page_content,
                "date": date_only
            }

        print(json.dumps(memories, indent=1))

        return json.dumps(memories, indent=1)
        
        
    
    




        
    
    
    
    
        



ltms = LongTermMemoryService()

# Current memories saved in the database to test the get_relevant_memories function
'''
ltms.save_as_longterm_memory("I am a student", "2022-03-01T12:00", ["student", "education", "technology"])
ltms.save_as_longterm_memory("I am a teacher", "2022-03-01T12:01", ["teacher", "education", "technology"])
ltms.save_as_longterm_memory("I am a doctor", "2022-03-01T12:02", ["doctor", "healthcare", "technology"])
ltms.save_as_longterm_memory("I am a nurse", "2022-03-01T12:03", ["nurse", "healthcare", "technology"])
ltms.save_as_longterm_memory("I am a software engineer", "2022-03-01T12:04", ["software engineer", "technology", "education"])
ltms.save_as_longterm_memory("I am a data scientist", "2022-03-01T12:05", ["data scientist", "technology", "education"])
ltms.save_as_longterm_memory("I am a data scientist", "2022-03-01T12:05", ["technology", "education", "data scientist"])
'''

# Test the get_relevant_memories function
'''
print(ltms.get_relevant_memories("I am a student", ["data scientist", "data scientist", "data scientist"]))
print(ltms.get_relevant_memories("I am a student"))
'''


