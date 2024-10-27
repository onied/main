import classes from "./chatBadgeList.module.css"

import IdBar from "@onied/components/general/idBar/idBar"
import combineCssClasses from "@onied/helpers/combineCssClasses"
import { ChatBadge } from "@onied/types/chat"
import { Side } from "@onied/types/general"

type Props = {
    title: string
    badges: ChatBadge[]
    side: Side
}

export default function ChatBadgeList({ title, badges, side }: Props) {
    return <nav className={combineCssClasses([
        classes.chatBadgeList,
        side == Side.Left ? classes.left : classes.right
    ])}>
        <div className={classes.chatBadgeListHeader}>{title}</div>
        {badges.map(badge => <ChatBadgeItem badge={badge} />)}
    </nav>
}

const UnreadCount = ({ count }: { count: number }) =>
    <span className={classes.unreadCount}>{count}</span>

function ChatBadgeItem({ badge }: { badge: ChatBadge }) {
    return <div className={classes.badgeItem}>
        <p>{badge.LastMessage.Message}</p>
        <div className={classes.badgeFooter}>
            <IdBar id={badge.ChatId} />
            <UnreadCount count={1} />
        </div>
    </div>
}