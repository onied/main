
import classes from "./activeArea.module.css"

import UpperBar from "./upperBar/upperBar"
import ChatArea from "./chatArea/chatArea"
import { Chat } from "@onied/types/chat"

export default function ActiveArea({ activeChat }: { activeChat: Chat }) {
    return <div className={classes.activeArea}>
        <UpperBar currentChat={activeChat}/>
        <ChatArea chat={activeChat} />
    </div>
}