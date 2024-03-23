import { useEffect, useState } from "react";
import EmbedVideo from "./embedVideo";
import BeatLoader from "react-spinners/BeatLoader";
import api from "../../../config/axios";

function Video({ courseId, blockId }) {
  const [videoBlock, setVideoBlock] = useState();
  const [found, setFound] = useState();

  useEffect(() => {
    setFound(undefined);
    axios
      .get(Config.CoursesBackend + "courses/" + courseId + "/video/" + blockId)
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setVideoBlock(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, [courseId, blockId]);
  if (found == null)
    return <BeatLoader color="var(--accent-color)"></BeatLoader>;
  if (!found) return <></>;
  return (
    <>
      <h2>{videoBlock.title}</h2>
      <EmbedVideo href={videoBlock.href}></EmbedVideo>
    </>
  );
}

export default Video;
