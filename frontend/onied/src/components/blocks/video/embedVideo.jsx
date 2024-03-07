import { useState } from "react";
import classes from "./embedVideo.module.css"

// 
const embedElements = [
    {
        // youtube
        regex: /^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w-]+)(\S+)?$/,
        getProvider: (href) => {
            const regex = /^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w]+)(\S+)?$/;
            const matches = href.match(regex);

            return <iframe 
            src={`https://www.youtube.com/embed/${matches.groups.videoId}`}
            className={classes.embedIFrame}
            title="YouTube video player"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" 
            allowFullScreen />;
        }
    },
    {
        // vk video
        regex: /^(?:https:\/\/)?(?:www\.)?(?:vk\.com|m\.vk\.com)(?:\/video)(?<videoOid>[\d-]+)_(?<videoId>[\d]+)$/,
        getProvider: (href) => {
            const regex = /^(?:https:\/\/)?(?:www\.)?(?:vk\.com|m\.vk\.com)(?:\/video)(?<videoOid>[\d-]+)_(?<videoId>[\d]+)$/;
            const matches = href.match(regex);
            
            return <iframe 
            src={`https://vk.com/video_ext.php?oid=${matches.groups.videoOid}&id=${matches.groups.videoId}`}
            className={classes.embedIFrame}
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"           
            allowFullScreen />;
        }
    },
    {
        // rutube
        regex: /^((?:https?:)?\/\/)?(rutube\.ru)(\/video)(?<videoId>\/[\w\d]+)?(\/[\S]+)?$$/,
        getProvider: (href) => {
            const regex = /^((?:https?:)?\/\/)?(rutube\.ru)(\/video)(?<videoId>\/[\w\d]+)(\/[\S]+)?$/;
            const matches = href.match(regex);
            // https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd
            
            
            return <iframe 
            src={`https://rutube.ru/play/embed/${matches.groups.videoId}`} 
            className={classes.embedIFrame}
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
            allowFullScreen />;
        }
    }
];

function videoLinkToEmbedLink(href) {
    const embedRegex = embedElements.filter((item) => item.regex.test(href));

    if (embedRegex.length == 0)
        throw Error("Неверный формат ссылки на видео");

    return embedRegex[0].getProvider(href);
}

function EmbedVideo({ href }) {

    const [videoProvider, setVideoProvider] = useState(videoLinkToEmbedLink(href));

    return <div className={classes.embedVideo}>
        {videoProvider}
    </div>;
}


export default EmbedVideo;