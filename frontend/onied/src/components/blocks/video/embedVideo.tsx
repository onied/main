import classes from "./embedVideo.module.css";
import VideoProvider from "./providers/videoProvider";
import YoutubeVideoProvider from "./providers/youtubeVideoProvider";
import VkVideoProvider from "./providers/vkVideoProvider";
import RutubeVideoProvider from "./providers/rutubeVideoProvider";
import FileVideoProvider from "./providers/fileProvider";
import { useEffect, useState } from "react";

const embedElements: VideoProvider[] = [
  new YoutubeVideoProvider(),
  new VkVideoProvider(),
  new RutubeVideoProvider(),
  new FileVideoProvider(),
];

function videoLinkToIFrame(href: string) {
  const embedRegex = embedElements.filter((item) => item.regex.test(href));
  if (embedRegex.length == 0)
    return (
      <div className={classes.embedVideo}>Неверный формат ссылки на видео</div>
    );
  const [iframeLink, setIframeLink] = useState<string | undefined>();
  const [rawVideo, setRawVideo] = useState<boolean>(false);
  useEffect(() => {
    setIframeLink(undefined);
    embedRegex[0].getLink(href).then((link) => setIframeLink(link));
    setRawVideo(embedRegex[0].rawVideo);
  }, [href]);

  if (!iframeLink) return <></>;
  if (rawVideo)
    return (
      <video controls className={classes.embedIFrame}>
        <source src={iframeLink}></source>
      </video>
    );
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
