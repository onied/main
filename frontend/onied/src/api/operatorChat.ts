import { UUID } from "crypto"

import api from "@onied/config/axios"
import { Chat, ChatBadge, OperatorProfile } from "@onied/types/chat"

export default class OperatorChatApi {

    private Get = async <TResponse>(url: string) => {
        return await api
        .get(url)
        .then((response) => {
            console.log(response)
            return response.data as TResponse
        })
        .catch((response) => {
            console.log(response)
            return null
        })
    }

    GetChat = async (chatId: UUID) => await this.Get<Chat>(`chat/${chatId}`)

    GetOperatorProfile = async () => await this.Get<OperatorProfile>("/support/profile")

    GetActiveChats = async () => await this.Get<ChatBadge[]>("/support/active")

    GetOpenChats = async () => await this.Get<ChatBadge[]>("/support/open")
} 