import classes from './chatsSidebar.module.css'

import combineCssClasses from '@onied/helpers/combineCssClasses'
import { Side } from '@onied/types/general'
import { ChatBadge } from '@onied/types/chat'
import ChatBadgeList from './chatBadgeList'

import { useState } from 'react'

type Props = {
    title: string
    badges: ChatBadge[]
    side: Side
}

export default function ChatsSidebar({ title, badges, side }: Props) {
    const [isOpen, setIsOpen] = useState(true)

    const toggleSidebar = () => setIsOpen(!isOpen)

    console.log(`isOpen: ${isOpen}`)

    return (
        <div className={combineCssClasses([
            classes.sidebar,
            side == Side.Left ? classes.left : classes.right,
            isOpen ? classes.open : null
        ])}>
            <div className={combineCssClasses([
                classes.sidebarButtonPanel,
                side == Side.Left ? classes.left : classes.right
            ])}>
                <button className={classes.toggleButton} onClick={toggleSidebar}><HaburgerIcon /></button>
            </div>
            <ChatBadgeList title={title} side={side} badges={badges} />
        </div>
    )
}

const HaburgerIcon = () => (
    <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
        <rect x="0" width="32" height="32" rx="2" fill="#ECECEC" />
        <path d="M7.16663 8H16.5H25.8333M7.16663 16H25.8333M7.16663 24H25.8333"
            stroke="#BBBBBB" strokeWidth="5.33333" strokeLinecap="round" />
    </svg>)