from langchain_chroma import Chroma
from langchain.docstore.document import Document
from langchain_huggingface import HuggingFaceEmbeddings


class LongTermMemoryService:

    vector_directory = "Data/VectorStore"
    embedding_function = HuggingFaceEmbeddings(model_name="intfloat/multilingual-e5-large")

    def __init__(self):
        pass

    
    def save_as_longterm_memory(self, memory: str, timestamp: str, list_of_tags: list[str]) -> None:

        documents = [Document(page_content=memory, metadata={"timestamp": timestamp, "tag1": list_of_tags[0], "tag2": list_of_tags[1], "tag3": list_of_tags[3]})]

        Chroma.from_documents(
        documents,
        self.embedding_function,
        persist_directory=self.vector_directory, # save in chromadb folder
    )
        
    def get_relevant_memories(self, query: str):
        database = Chroma(persist_directory=self.vector_directory, embedding_function=self.embedding_function) # load from the saved folder

        relevant_documents = database.similarity_search(
            query="I am a student",
            k=5,
            filter={"tags": {"$in": ["student"]}} # filter by tag
            ) 
        

        return relevant_documents
        



ltms = LongTermMemoryService()
'''
ltms.save_as_longterm_memory("I am a student", "2022-03-01T12:00", ["student", "education"])
ltms.save_as_longterm_memory("I am a teacher", "2022-03-01T12:01", ["teacher", "education"])
ltms.save_as_longterm_memory("I am a doctor", "2022-03-01T12:02", ["doctor", "healthcare"])
ltms.save_as_longterm_memory("I am a nurse", "2022-03-01T12:03", ["nurse", "healthcare"])
ltms.save_as_longterm_memory("I am a software engineer", "2022-03-01T12:04", ["software engineer", "technology"])
ltms.save_as_longterm_memory("I am a data scientist", "2022-03-01T12:05", ["data scientist", "technology"])
ltms.save_as_longterm_memory("I am a data scientist", "2022-03-01T12:05", ["technology"])
'''

print(ltms.get_relevant_memories("I am a student"))

