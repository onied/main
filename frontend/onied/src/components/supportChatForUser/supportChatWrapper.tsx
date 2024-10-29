import { useEffect, useState } from "react";
import api from "../../config/axios";
import ChatButton from "@onied/components/supportChatForUser/chatButton/chatButton";
import ChatWindow from "@onied/components/supportChatForUser/chatWindow/chatWindow";
import { MessagesHistoryDto } from "@onied/components/supportChatForUser/messageDtos";
import GetHistory from "@onied/components/supportChatForUser/tempMessagesSource";
import { AxiosError } from "axios";
import { useProfile } from "@onied/hooks/profile/useProfile";

function SupportChatWrapper() {
  const [profile, _] = useProfile();
  const [isChatWindowOpen, setIsChatWindowOpen] = useState(false);
  const [isFirstEverMessage, setIsFirstEverMessage] = useState(false);
  const [messagesHistory, setMessagesHistory] = useState<MessagesHistoryDto>();

  useEffect(() => {
    api
      .get("/chat")
      .then((response) => {
        setMessagesHistory(response.data);
      })
      .catch((error: AxiosError<any, any>) => {
        if (error.response != null) {
          if (error.response.status == 404) {
            setIsFirstEverMessage(true);
          }
        } else {
          console.error("Could not reach the server.");
          setMessagesHistory(GetHistory(0));
        }
      });
  }, []);

  if (profile === null) return <></>;

  return (
    <div
      style={{
        display: "inline-block",
        position: "fixed",
        maxWidth: "22vw",
        minWidth: "20vw",
        padding: "20px",
        right: 0,
        bottom: 0,
        zIndex: 9999,
      }}
    >
      <ChatWindow
        isChatWindowOpen={isChatWindowOpen}
        isFirstEverMessage={isFirstEverMessage}
        messagesHistory={
          messagesHistory
            ? messagesHistory
            : ({
                supportNumber: null,
                currentSessionId: null,
                messages: [],
              } as MessagesHistoryDto)
        }
        setMessagesHistory={setMessagesHistory}
      ></ChatWindow>
      <ChatButton
        isChatWindowOpen={isChatWindowOpen}
        setIsChatWindowOpen={setIsChatWindowOpen}
      ></ChatButton>
    </div>
  );
}

export default SupportChatWrapper;
