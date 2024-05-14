import { useParams } from "react-router-dom";
import Summary from "./summary/summary";
import Tasks from "./tasks/tasks";
import Video from "./video/video";
import { useEffect } from "react";
import classes from "./blocks.module.css";
import NotFound from "../general/responses/notFound/notFound";
import CustomBeatLoader from "../general/customBeatLoader";

function BlockDispatcher({ hierarchy, setCurrentBlock }) {
  const { blockId } = useParams();
  const blockTypes = [
    <></>,
    <Summary courseId={hierarchy?.id} blockId={Number(blockId)} />,
    <Video courseId={hierarchy?.id} blockId={Number(blockId)} />,
    <Tasks courseId={hierarchy?.id} blockId={Number(blockId)} />,
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
    return <NotFound>Блок не найден.</NotFound>;
  useEffect(() => setCurrentBlock(Number(blockId)));

  return (
    <div className={classes.blockWrapper}>
      {blocks ? blockTypes[blocks[blockId].blockType] : <CustomBeatLoader />}
    </div>
  );
}

export default BlockDispatcher;
