import { useState } from "react";
import classes from "./embedVideo.module.css"

// 
const embedElements = [
    {
        // youtube
        regex: /^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w-]+)(\S+)?$/,
        getLink: (href) => {
            const regex = /^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w]+)(\S+)?$/;
            const matches = href.match(regex);

            return `https://www.youtube.com/embed/${matches.groups.videoId}`;
        }
    },
    {
        // vk video
        regex: /^(?:https:\/\/)?(?:www\.)?(?:vk\.com|m\.vk\.com)(?:\/video)(?<videoOid>[\d-]+)_(?<videoId>[\d]+)$/,
        getLink: (href) => {
            const regex = /^(?:https:\/\/)?(?:www\.)?(?:vk\.com|m\.vk\.com)(?:\/video)(?<videoOid>[\d-]+)_(?<videoId>[\d]+)$/;
            const matches = href.match(regex);
            
            return `https://vk.com/video_ext.php?oid=${matches.groups.videoOid}&id=${matches.groups.videoId}`;
        }
    },
    {
        // rutube
        regex: /^((?:https?:)?\/\/)?(rutube\.ru)(\/video)(?<videoId>\/[\w\d]+)?(\/[\S]+)?$$/,
        getLink: (href) => {
            const regex = /^((?:https?:)?\/\/)?(rutube\.ru)(\/video)(?<videoId>\/[\w\d]+)(\/[\S]+)?$/;
            const matches = href.match(regex);
            
            return `https://rutube.ru/play/embed/${matches.groups.videoId}`;
        }
    }
];

function videoLinkToVideoProvider(href) {
    const embedRegex = embedElements.filter((item) => item.regex.test(href));

    const iframeLink = embedRegex[0].getLink(href); 
    if (embedRegex.length == 0)
        throw Error("Неверный формат ссылки на видео");

    return <iframe 
    src={iframeLink} 
    className={classes.embedIFrame}
    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
    allowFullScreen />;
}

function EmbedVideo({ href }) {

    const [videoProvider, setVideoProvider] = useState(videoLinkToVideoProvider(href));

    return <div className={classes.embedVideo}>
        {videoProvider}
    </div>;
}


export default EmbedVideo;