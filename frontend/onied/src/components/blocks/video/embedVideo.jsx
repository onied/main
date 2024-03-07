import { useState } from "react";
import classes from "./embedVideo.module.css"

const embedElements = [
    {
        regex: /^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w-]+)(\S+)?$/,
        getProvider: (href) => {
            const regex = /^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w]+)(\S+)?$/;
            const matches = href.match(regex);
            console.log(`https://www.youtube.com/embed/${matches.groups.videoId}`);

            return <iframe 
            src={`https://www.youtube.com/embed/${matches.groups.videoId}`}
            className={classes.embedIFrame}
            title="YouTube video player"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" 
            allowfullscreen />;
        }
    },
    {
        regex: /^((?:https?:)?\/\/)?(vk\.com|m\.vk\.com|)?(\/video(?<videoOid>[\d-]+)_(?<videoId>[\d]+))?$/,
        getProvider: (href) => {
            const regex = /^((?:https?:)?\/\/)?(vk\.com|m\.vk\.com|)?(\/video(?<videoOid>[\d-]+)_(?<videoId>[\d]+))?$/;
            const matches = href.match(regex);
            

            // нужен хэш
            console.log(`https://vk.com/video_ext.php?oid=${matches.groups.videoOid}&id=${matches.groups.videoId}&hd=2`);
            return <iframe 
            src={`https://vk.com/video_ext.php?oid=${matches.groups.videoOid}&id=${matches.groups.videoId}&hd=2`}
            className={classes.embedIFrame}
            allow="autoplay; encrypted-media; picture-in-picture;" 
            allowFullScreen />;
            // return <iframe src="https://vk.com/video_ext.php?oid=197465133&id=456239888&hd=2&hash=42d236acf4683c78" width="853" height="480" allow="autoplay; encrypted-media; fullscreen; picture-in-picture;" allowFullScreen></iframe>
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