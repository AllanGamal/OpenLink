import { useState } from "react";

import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";
import FileContainerComponent from "./components/FileContainer/FileContainerComponent/FileContainerComponent";
import ChatContainerComponent from "./components/ChatContainer/ChatContainerComponent/ChatContainerComponent";

function App() {
  const [greetMsg, setGreetMsg] = useState("");
  const [name, setName] = useState("");

  
/*
  return (
    <div className="main-container">
      <FileContainerComonent />
      <ChatContainerComponent />

    </div>
  );
  */
 return (
   <div className="main-container">
     <ChatContainerComponent />

   </div>
 );
}

export default App;
