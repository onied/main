import { Side } from "@onied/types/general";
import OperatorsHeader from "./components/operatorsHeader/operatorsHeader";
import ChatsSidebar from "./components/ChatsSidebar/chatsSidebar";
import Background from "./components/background/background";
import ActiveArea from "./components/activeArea/activeArea";
import { useAppSelector } from "@onied/hooks";

export default function OperatorsPage() {
    const chatsState = useAppSelector((state) => state.chats);
    console.log(chatsState)

    return <>
        <OperatorsHeader />
        <main>
            <Background />
            <ActiveChatsSidebar />
            {chatsState.currentChat && <ActiveArea activeChat={chatsState.currentChat} />}
            <OpenChatsSidebar />
        </main>
    </>
}

const ActiveChatsSidebar = () => {
    const chatsState = useAppSelector((state) => state.chats);
    const badges = chatsState.activeChats
    return <ChatsSidebar title={"Активные обращения"} badges={badges} side={Side.Left} searchEnabled={false} />
}

const OpenChatsSidebar = () => {
    const chatsState = useAppSelector((state) => state.chats);
    const badges = chatsState.openChats
    return <ChatsSidebar title={"Открытые обращения"} badges={badges} side={Side.Right} searchEnabled={true} />
}
