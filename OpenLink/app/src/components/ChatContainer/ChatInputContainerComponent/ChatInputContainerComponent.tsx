import './ChatInputContainerComponent.css';
import React, { useState } from 'react';
import axios from 'axios';
import { Message } from '../ChatHistoryContainerComponent/ChatHistoryContainerComponent';

interface Props {
  // message and type 
  onSendMessage: (message: Message) => void;

}



function ChatInputContainerComponent({ onSendMessage }: Props) {
  const [message, setMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isEnglish, setIsEnglish] = useState(true);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setMessage(event.target.value);
  };

  const toggleLanguage = () => {
    setIsEnglish(!isEnglish)
  }


  const handleSendClick = () => {
    const apiUrl = 'http://localhost:8001/message';


    if (message !== '') {
      onSendMessage({ text: message, type: 'user'});
      onSendMessage({ text: "message", type: 'bot'});
      setMessage('');
    }
  };

  return (
    <div className="chat-input-container" >
      <div className="chat-input-element-container">
        <input
          type="text"
          className="chat-input"
          placeholder="Ask a question..."
          value={message}
          onChange={handleInputChange}
          onKeyPress={(event) => {
            if (event.key === 'Enter') {
              handleSendClick();
            }
          }}

        >

        </input>
        
        <button type="button" className="btn btn-primary" onClick={handleSendClick} disabled={isLoading} >
          {isLoading ? '....' : 'Send'}
        </button>
        
      </div>
    </div>
  );
}

export default ChatInputContainerComponent;