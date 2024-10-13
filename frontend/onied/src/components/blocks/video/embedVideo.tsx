import classes from "./embedVideo.module.css";
import VideoProvider from "./providers/videoProvider";
import YoutubeVideoProvider from "./providers/youtubeVideoProvider";
import VkVideoProvider from "./providers/vkVideoProvider";
import RutubeVideoProvider from "./providers/rutubeVideoProvider";

const embedElements: VideoProvider[] = [
  new YoutubeVideoProvider(),
  new VkVideoProvider(),
  new RutubeVideoProvider(),
];

function videoLinkToIFrame(href: string) {
  const embedRegex = embedElements.filter((item) => item.regex.test(href));
  if (embedRegex.length == 0)
    return (
      <div className={classes.embedVideo}>
        Неверный формат ссылки на видео
      </div>);
  const iframeLink = embedRegex[0].getLink(href);

  return (
    <iframe
      data-testid="iframe-video"
      src={iframeLink}
      className={classes.embedIFrame}
      allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
      allowFullScreen
    />
  );
}

function EmbedVideo({ href }: { href: string }) {
  const videoProvider = videoLinkToIFrame(href);

  return <div className={classes.embedVideo}>{videoProvider}</div>;
}

export default EmbedVideo;
