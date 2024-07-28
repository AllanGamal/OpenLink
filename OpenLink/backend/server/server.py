from flask import Flask, request, jsonify
from flask_cors import CORS

app = Flask(__name__)
CORS(app)  # Allow all domains

@app.route('/LLM/query', methods=['POST'])
def recieve_question():
    data = request.get_json()
    query = data.get('query', '') # the question to be processed by the llm

    response = {
        'response': f'Received query: {query}'
    }
    return jsonify(response)

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5105)