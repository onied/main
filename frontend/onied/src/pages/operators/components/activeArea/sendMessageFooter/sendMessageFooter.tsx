
import classes from "./sendMessageFooter.module.css"

import InputFormArea from "@onied/components/general/inputform/inputFormArea"

import { MouseEventHandler } from "react"

const SendButton = ({ onClick }: { onClick?: MouseEventHandler }) => {
    return (
        <svg onClick={onClick} width="40" height="40" viewBox="0 0 40 40" fill="none" xmlns="http://www.w3.org/2000/svg">
            <rect width="40" height="40" rx="20" fill="#9715D3" />
            <path d="M12.71 14.2706L24.03 10.4973C29.11 8.80396 31.87 11.5773 30.19 16.6573L26.4167 27.9772C23.8833 35.5906 19.7233 35.5906 17.19 27.9772L16.07 24.6172L12.71 23.4972C5.09667 20.9639 5.09667 16.8173 12.71 14.2706Z" stroke="white" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" />
            <path d="M16.3233 24.0441L21.0967 19.2574" stroke="white" stroke-width="4" stroke-linecap="round" stroke-linejoin="round" />
        </svg>)
}

export default function SendMessageFooter() {
    return <div className={classes.chatFooter}>
        <InputFormArea style={{ "width": "100%", "height": "40px", "resize": "none" }}/>
        <SendButton />
    </div>
}