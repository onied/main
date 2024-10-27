import { Side } from "@onied/types/general";
import { ChatBadge } from "@onied/types/chat";
import OperatorsHeader from "./components/operatorsHeader/operatorsHeader";
import ChatsSidebar from "./components/ChatsSidebar/chatsSidebar";
import Background from "./components/background/background";

const defaultBadges: ChatBadge[] = [
    {
        ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        LastMessage: {
            MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
            SupportNumber: 69,
            CreatedAt: 0,
            ReadAt: null,
            IsSystem: false,
            Message: "А что кушают поросятки?"
        }
    },
    {
        ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        LastMessage: {
            MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
            SupportNumber: 69,
            CreatedAt: 0,
            ReadAt: null,
            IsSystem: false,
            Message: "А что кушают поросятки?"
        }
    },
    {
        ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        LastMessage: {
            MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
            SupportNumber: 69,
            CreatedAt: 0,
            ReadAt: null,
            IsSystem: false,
            Message: "А что кушают поросятки?"
        }
    }
]

export default function OperatorsPage() {
    return <>
        <OperatorsHeader />
        <main>
            <Background />
            <ActiveChatsSidebar />
            <OpenChatsSidebar />
        </main>
    </>
}

const ActiveChatsSidebar = () => {
    const badges = defaultBadges
    return <ChatsSidebar title={"Активные обращения"} badges={badges} side={Side.Left} />
}

const OpenChatsSidebar = () => {
    const badges = defaultBadges
    return <ChatsSidebar title={"Открытые обращения"} badges={badges} side={Side.Right} />
}
