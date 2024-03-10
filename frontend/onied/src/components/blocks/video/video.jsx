import { useState } from "react";
import EmbedVideo from "./embedVideo";
import classes from "./embedVideo.module.css";

function Video() {
  const [videoBlock, setVideoBlock] = useState({
    title: "MAKIMA BEAN",
    href: "https://www.youtube.com/watch?v=YfBlwC44gDQ",
  });
  return (
    <>
      <h2>{videoBlock.title}</h2>
      <EmbedVideo href={videoBlock.href}></EmbedVideo>
    </>
  );
}

export default Video;
