import classes from "./chatBadgeList.module.css";

import IdBar from "@onied/components/general/idBar/idBar";
import combineCssClasses from "@onied/helpers/combineCssClasses";
import { ChatsStateActionTypes } from "@onied/redux/reducers/chatReducer";
import { Chat, ChatBadge } from "@onied/types/chat";
import { Side } from "@onied/types/general";
import { useAppDispatch } from "@onied/hooks";

import { useState } from "react";
import InputForm from "@onied/components/general/inputform/inputform";

// import OperatorChatApi from '@onied/api/operatorChat'

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
        const badgeId = badge.ChatId.replaceAll("-", "").toLowerCase();
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
          key={`${title[0]}-chat-badge-${badge.ChatId}`}
        />
      ))}
    </nav>
  );
}

const UnreadCount = ({ count }: { count: number }) => (
  <span className={classes.unreadCount}>{count}</span>
);

const currentChat: Chat = {
  SupportNumber: 69,
  CurrentSessionId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
  Messages: [
    {
      MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
      SupportNumber: null,
      CreatedAt: 1730115891,
      ReadAt: 1730115891,
      IsSystem: false,
      Message:
        "бла-бла-бла esfesfdsfds esfesfdsfds esfesfdsfds asda dsadsa da sadas",
    },
    {
      MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
      SupportNumber: 69,
      CreatedAt: 1730115891,
      ReadAt: 1730115891,
      IsSystem: false,
      Message:
        "бла-бла-бла esfesfdsfds esfesfdsfds esfesfdsfds asda dsadsa da sadas",
    },
  ],
};

function ChatBadgeItem({ badge }: { badge: ChatBadge }) {
  const dispatch = useAppDispatch();
  // const operatorChatApi = new OperatorChatApi()

  const openChatEvent = () => {
    // const currentChat = operatorChatApi.GetChat(badge.ChatId)
    dispatch({
      type: ChatsStateActionTypes.FETCH_CURRENT_CHAT,
      payload: currentChat,
    });
  };

  return (
    <div className={classes.badgeItem} onClick={openChatEvent}>
      <p>{badge.LastMessage.Message}</p>
      <div className={classes.badgeFooter}>
        <IdBar id={badge.ChatId} />
        <UnreadCount count={1} />
      </div>
    </div>
  );
}
