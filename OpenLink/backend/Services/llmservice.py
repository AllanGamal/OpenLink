
from langchain_community.llms import Ollama
from langchain_community.chat_models import ChatOllama
from langchain_community.vectorstores import Chroma

from langchain_community.embeddings.sentence_transformer import (
    SentenceTransformerEmbeddings,
)
from langchain_community.vectorstores import Chroma
from langchain.chains import RetrievalQA
from langchain.docstore.document import Document
from sentence_transformers import SentenceTransformer



def askLLMAndGetResponse(question, llm):
    llm = Ollama(model=llm)
    response = llm.invoke(question)
    return response

