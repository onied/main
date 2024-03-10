import { useParams } from "react-router-dom";
import Summary from "./summary/summary";
import Tasks from "./tasks/tasks";
import Video from "./video/video";
import { useEffect } from "react";

function BlockDispatcher({ hierarchy, setCurrentBlock }) {
  const { blockId } = useParams();
  const blockTypes = [<></>, <Summary />, <Video />, <Tasks />];
  const blocks =
    hierarchy != null && "modules" in hierarchy
      ? hierarchy.modules
          .flatMap((module) => module.blocks)
          .reduce((acc, cur) => ({ ...acc, [cur.id]: cur }), {})
      : undefined;
  if (isNaN(Number(blockId))) return <></>;
  useEffect(() => setCurrentBlock(Number(blockId)));

  return <>{blocks ? blockTypes[blocks[blockId].blockType] : <></>}</>;
}

export default BlockDispatcher;
