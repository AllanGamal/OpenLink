from flask import Flask, request, jsonify
from flask_cors import CORS
from Services.LLMService import LLMService
from Services.ShortTermMemoryService import ShortTermMemoryService
from Controllers.LLMController import LLMController
import os

app = Flask(__name__)
CORS(app)  # Allow all domains

short_term_memory_service = ShortTermMemoryService()
llm_service = LLMService(short_term_memory_service)
llm_controller = LLMController(llm_service)

@app.route('/LLM/query', methods=['POST'])
def recieve_question():
    data = request.get_json()
    query = data.get('query', '') # the question to be processed by the llm

    response = {
        'response':  f'{llm_controller.start_conversation_mode(query)}'
    }
    return jsonify(response)
    
    



if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5105)