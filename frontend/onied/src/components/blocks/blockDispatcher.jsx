import { useParams } from "react-router-dom";
import Summary from "./summary/summary";
import Tasks from "./tasks/tasks";
import Video from "./video/video";
import { useEffect } from "react";
import BeatLoader from "react-spinners/BeatLoader";
import classes from "./blocks.module.css";

function BlockDispatcher({ hierarchy, setCurrentBlock }) {
  const { blockId } = useParams();
  const blockTypes = [
    <></>,
    <Summary courseId={hierarchy?.id} blockId={Number(blockId)} />,
    <Video courseId={hierarchy?.id} blockId={Number(blockId)} />,
    <Tasks />,
  ];
  const blocks =
    hierarchy != null && "modules" in hierarchy
      ? hierarchy.modules
          .flatMap((module) => module.blocks)
          .reduce((acc, cur) => ({ ...acc, [cur.id]: cur }), {})
      : undefined;
  if (
    isNaN(Number(blockId)) ||
    (blocks != null && !Object.keys(blocks).includes(blockId))
  )
    return <h1 style={{ margin: "3rem" }}>Блок не найден.</h1>;
  useEffect(() => setCurrentBlock(Number(blockId)));

  return (
    <div className={classes.blockWrapper}>
      {blocks ? (
        blockTypes[blocks[blockId].blockType]
      ) : (
        <BeatLoader color="var(--accent-color)"></BeatLoader>
      )}
    </div>
  );
}

export default BlockDispatcher;
