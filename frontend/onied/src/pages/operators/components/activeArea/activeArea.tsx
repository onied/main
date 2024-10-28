
import classes from "./activeArea.module.css"

import UpperBar from "./upperBar/upperBar"
import ChatArea from "./chatArea/chatArea"
import { Chat } from "@onied/types/chat"

const defaultChat: Chat = {
    SupportNumber: 69,
    CurrentSessionId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
    Messages: [
        {
            MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
            SupportNumber: null,
            CreatedAt: 1730115891,
            ReadAt: 1730115891,
            IsSystem: false,
            Message: "бла-бла-бла esfesfdsfds esfesfdsfds esfesfdsfds asda dsadsa da sadas"
        },
        {
            MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
            SupportNumber: 69,
            CreatedAt: 1730115891,
            ReadAt: 1730115891,
            IsSystem: false,
            Message: "бла-бла-бла esfesfdsfds esfesfdsfds esfesfdsfds asda dsadsa da sadas"
        }
    ]
}

export default function ActiveArea() {
    const chat = defaultChat

    return <div className={classes.activeArea}>
        <UpperBar />
        <ChatArea chat={chat} />
    </div>
}