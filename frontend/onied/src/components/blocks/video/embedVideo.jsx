import classes from "./embedVideo.module.css";
import YoutubeVideoProvider from "./youtubeVideoProvider";
import VkVideoProvider from "./vkVideoProvider";
import RutubeVideoProvider from "./rutubeVideoProvider";

const embedElements = [
  new YoutubeVideoProvider(),
  new VkVideoProvider(),
  new RutubeVideoProvider(),
];

function videoLinkToIFrame(href) {
  const embedRegex = embedElements.filter((item) => item.regex.test(href));

  const iframeLink = embedRegex[0].getLink(href);
  if (embedRegex.length == 0) throw Error("Неверный формат ссылки на видео");

  return (
    <iframe
      src={iframeLink}
      className={classes.embedIFrame}
      allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
      allowFullScreen
    />
  );
}

function EmbedVideo({ href }) {
  const videoProvider = videoLinkToIFrame(href);

  return <div className={classes.embedVideo}>{videoProvider}</div>;
}

export default EmbedVideo;
