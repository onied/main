import classes from "./chatBadgeList.module.css";

import IdBar from "@onied/components/general/idBar/idBar";
import combineCssClasses from "@onied/helpers/combineCssClasses";
import { ChatsStateActionTypes } from "@onied/redux/reducers/chatReducer";
import { ChatBadge } from "@onied/types/chat";
import { Side } from "@onied/types/general";
import { useAppDispatch } from "@onied/hooks";

import { useState } from "react";
import InputForm from "@onied/components/general/inputform/inputform";

import OperatorChatApi from "@onied/api/operatorChat";

type Props = {
  title: string;
  badges: ChatBadge[];
  side: Side;
  searchEnabled: boolean;
};

export default function ChatBadgeList({
  title,
  badges,
  side,
  searchEnabled,
}: Props) {
  const [filteredBadges, setFilteredBadges] = useState<ChatBadge[]>(badges);

  const searchChat = (query: string) => {
    const id = query.replaceAll("-", "").toLowerCase() ?? "";

    setFilteredBadges(
      badges.filter((badge) => {
        const badgeId = badge.chatId.replaceAll("-", "").toLowerCase();
        return badgeId.startsWith(id);
      })
    );
  };

  return (
    <nav
      className={combineCssClasses([
        classes.chatBadgeList,
        side == Side.Left ? classes.left : classes.right,
      ])}
    >
      <div className={classes.chatBadgeListHeader}>
        <p>{title}</p>
        {searchEnabled && (
          <InputForm
            onInput={(e: any) => searchChat(e.target.value)}
            style={{ width: "100%", height: "40px", resize: "none" }}
          />
        )}
      </div>

      {filteredBadges.map((badge) => (
        <ChatBadgeItem
          badge={badge}
          key={`${title[0]}-chat-badge-${badge.chatId}`}
        />
      ))}
    </nav>
  );
}

const UnreadCount = ({ count }: { count: number }) => (
  <span className={classes.unreadCount}>{count}</span>
);

function ChatBadgeItem({ badge }: { badge: ChatBadge }) {
  const dispatch = useAppDispatch();
  const operatorChatApi = new OperatorChatApi();
  const [hidden, setHidden] = useState(false);

  const openChatEvent = () => {
    operatorChatApi
      .GetChat(badge.chatId)
      .then((currentChat) => {
        dispatch({
          type: ChatsStateActionTypes.FETCH_CURRENT_CHAT,
          payload: { chat: currentChat, chatId: badge.chatId },
        });
      })
      .catch(() => {
        dispatch({ type: ChatsStateActionTypes.FETCH_ACTIVE_CHATS });
        dispatch({ type: ChatsStateActionTypes.FETCH_OPEN_CHATS });
        setHidden(true);
      });
  };

  if (hidden) return <></>;

  return (
    <div className={classes.badgeItem} onClick={openChatEvent}>
      <p>{badge.lastMessage.message}</p>
      <div className={classes.badgeFooter}>
        <IdBar id={badge.chatId} />
        <UnreadCount count={1} />
      </div>
    </div>
  );
}
