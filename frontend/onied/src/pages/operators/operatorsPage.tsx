import { Side } from "@onied/types/general";
import OperatorsHeader from "./components/operatorsHeader/operatorsHeader";
import ChatsSidebar from "./components/ChatsSidebar/chatsSidebar";
import Background from "./components/background/background";
import ActiveArea from "./components/activeArea/activeArea";
import { useAppDispatch, useAppSelector } from "@onied/hooks";
import { useEffect } from "react";
import OperatorChatApi from "@onied/api/operatorChat";
import { ChatsStateActionTypes } from "@onied/redux/reducers/chatReducer";

export default function OperatorsPage() {
  const chatsState = useAppSelector((state) => state.chats);
  console.log(chatsState);

  return (
    <>
      <OperatorsHeader />
      <main>
        <Background />
        <ActiveChatsSidebar />
        {chatsState.currentChat && chatsState.currentChatId && (
          <ActiveArea
            activeChat={chatsState.currentChat}
            currentChatId={chatsState.currentChatId}
          />
        )}
        <OpenChatsSidebar />
      </main>
    </>
  );
}

const ActiveChatsSidebar = () => {
  const dispatch = useAppDispatch();
  const chatsState = useAppSelector((state) => state.chats);
  const badges = chatsState.activeChats;
  const opApi = new OperatorChatApi();
  useEffect(() => {
    opApi
      .GetActiveChats()
      .then((chats) => {
        dispatch({
          type: ChatsStateActionTypes.FETCH_ACTIVE_CHATS,
          payload: chats,
        });
      })
      .catch((error) => {
        console.error(error);
        dispatch({
          type: ChatsStateActionTypes.FETCH_ACTIVE_CHATS,
        });
      });
  }, []);
  return (
    <ChatsSidebar
      title={"Активные обращения"}
      badges={badges}
      side={Side.Left}
      searchEnabled={false}
    />
  );
};

const OpenChatsSidebar = () => {
  const dispatch = useAppDispatch();
  const chatsState = useAppSelector((state) => state.chats);
  const badges = chatsState.openChats;
  const opApi = new OperatorChatApi();
  useEffect(() => {
    opApi
      .GetOpenChats()
      .then((chats) => {
        dispatch({
          type: ChatsStateActionTypes.FETCH_OPEN_CHATS,
          payload: chats,
        });
      })
      .catch((error) => {
        console.error(error);
        dispatch({
          type: ChatsStateActionTypes.FETCH_OPEN_CHATS,
        });
      });
  }, []);
  return (
    <ChatsSidebar
      title={"Открытые обращения"}
      badges={badges}
      side={Side.Right}
      searchEnabled={true}
    />
  );
};
