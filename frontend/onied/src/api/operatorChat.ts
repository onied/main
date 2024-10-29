import { UUID } from "crypto";

import api from "@onied/config/axios";
import { Chat, ChatBadge, OperatorProfile } from "@onied/types/chat";

export default class OperatorChatApi {
  private Get = async <TResponse>(url: string) => {
    return await api
      .get(url)
      .then((response) => {
        console.log(response);
        return response.data as TResponse;
      })
      .catch((error) => {
        console.error(error);
        throw error;
      });
  };

  GetChat = async (chatId: UUID) => {
    let data = await this.Get<Chat>(`chat/${chatId}`);
    data.messages = data.messages.map((message) => {
      message.createdAt = new Date(message.createdAt);
      return message;
    });
    return data;
  };

  GetOperatorProfile = async () =>
    await this.Get<OperatorProfile>("/support/profile");

  GetActiveChats = async () => {
    let data = await this.Get<ChatBadge[]>("/support/active");
    data = data.map((chat) => {
      chat.lastMessage.createdAt = new Date(chat.lastMessage.createdAt);
      return chat;
    });
    return data;
  };

  GetOpenChats = async () => {
    let data = await this.Get<ChatBadge[]>("/support/open");
    data = data.map((chat) => {
      chat.lastMessage.createdAt = new Date(chat.lastMessage.createdAt);
      return chat;
    });
    return data;
  };
}
