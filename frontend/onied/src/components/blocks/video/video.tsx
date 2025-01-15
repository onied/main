import { useEffect, useState } from "react";
import EmbedVideo from "@onied/components/blocks/video/embedVideo";
import api from "@onied/config/axios";
import CustomBeatLoader from "@onied/components/general/customBeatLoader";
import { VideoBlock } from "@onied/types/block";
import FileMetadata from "@onied/components/general/fileMetadata/fileMetadata";

type props = {
  courseId: number;
  blockId: number;
};

function Video({ courseId, blockId }: props) {
  const [videoBlock, setVideoBlock] = useState<VideoBlock>();
  const [found, setFound] = useState<boolean>();

  useEffect(() => {
    api
      .get("courses/" + courseId + "/video/" + blockId)
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

  if (found === undefined) return <CustomBeatLoader />;
  if (!found) return <></>;
  return (
    <>
      <h2>{videoBlock!.title}</h2>
      <EmbedVideo href={videoBlock!.href}></EmbedVideo>
      <FileMetadata fileId={videoBlock!.href}></FileMetadata>
    </>
  );
}

export default Video;
