import { useState } from "react";
import classes from "./embedVideo.module.css"

/**
 * Abstract Class VideoProvider.
 *
 * @class VideoProvider
 */
class VideoProvider
{   
    constructor() {
        if (this.constructor == VideoProvider) {
            throw new Error("Abstract classes can't be instantiated.");
        }
    }

    // regex: регулярное выражение, соответствующее видеопровайдеру.
    regex = new RegExp();
    
    // getIFrameLink: функция, которая конвертирует ссылку в ссылку для iframe.
    // regex должен содержать именованные группы, содержащиеся в convertHrefToIFrameSrc.
    getLink(href) {
        throw new Error("Method 'getLink()' must be implemented.");
    }
}

class YoutubeVideoProvider
{   
    constructor() {}

    regex = /^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w]+)(\S+)?$/;
    
    getLink(href) {
        const matches = href.match(this.regex);

        return `https://www.youtube.com/embed/${matches.groups.videoId}`;
    }
}

class VkVideoProvider
{   
    constructor() {}

    regex = /^(?:https:\/\/)?(?:www\.)?(?:vk\.com|m\.vk\.com)(?:\/video)(?<videoOid>[\d-]+)_(?<videoId>[\d]+)$/;
    
    getLink(href) {
        const matches = href.match(this.regex);

        return `https://vk.com/video_ext.php?oid=${matches.groups.videoOid}&id=${matches.groups.videoId}`;
    }
}

class RutubeVideoProvider
{   
    constructor() {}

    regex = /^((?:https?:)?\/\/)?(rutube\.ru)(\/video)(?<videoId>\/[\w\d]+)?(\/[\S]+)?$/;
    
    getLink(href) {
        const matches = href.match(this.regex);
        
        return `https://rutube.ru/play/embed/${matches.groups.videoId}`;
    }
}

// 
const embedElements = [
    new YoutubeVideoProvider(),
    new VkVideoProvider(),
    new RutubeVideoProvider(),
];

function videoLinkToIFrame(href) {
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

    const [videoProvider, setVideoProvider] = useState(videoLinkToIFrame(href));

    return <div className={classes.embedVideo}>
        {videoProvider}
    </div>;
}


export default EmbedVideo;