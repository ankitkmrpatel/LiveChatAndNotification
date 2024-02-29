"use client";

import { createContext, useRef, useState } from "react";

export const EventMessageContext = createContext();

export const EventMessageProvider = ({ children }) => {
  const [messages, setMessages] = useState([]);
  const [socketConnection, setSocketConnection] = useState(null);
  const [isMessageLoaded, setIsMessageLoaded] = useState(false);

  const addNewMessages = (chatDatas) => {
    console.log("Hello Messages Called", chatDatas);
    setMessages((prev) => {
      console.log("Hello Message Called", prev);
      let allMessage = [...prev, ...chatDatas];
      console.log("Hello Message Called", allMessage);
      return allMessage;
    });
  };

  return (
    <EventMessageContext.Provider
      value={{
        messages,
        setMessages,
        addNewMessages,
        socketConnection,
        setSocketConnection,
        setIsMessageLoaded,
        isMessageLoaded,
      }}
    >
      {children}
    </EventMessageContext.Provider>
  );
};
