import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { EditBlockDispathcer } from "../../components/editCourse/editBlocks";
import api from "../../config/axios";
import { BlockInfo } from "../../types/course";
import NotFound from "../../components/general/responses/notFound/notFound";
import Forbid from "../../components/general/responses/forbid/forbid";
import CustomBeatLoader from "../../components/general/customBeatLoader";

function EditBlock() {
  const { courseId, blockId } = useParams();
  const [found, setFound] = useState<boolean | undefined>();
  const [block, setBlock] = useState<BlockInfo | null | undefined>(undefined);

  const parsedCourseId = Number(courseId);
  const parsedBlockId = Number(blockId);
  const notFound = <NotFound>Курс или блок не найден.</NotFound>;
  const [isForbid, setIsForbid] = useState(false);
  const forbid = <Forbid>Вы не можете редактировать данный курс.</Forbid>;

  useEffect(() => {
    setBlock(null);
    if (isNaN(parsedCourseId) || isNaN(parsedBlockId)) {
      setFound(false);
      return;
    }
    api
      .get("courses/" + courseId + "/edit/check-edit-course")
      .then((_) => {
        api
          .get("courses/" + courseId + "/hierarchy")
          .then((response) => {
            const block = response.data.modules
              .map((m: any) => m.blocks)
              .reduce((x: BlockInfo[], y: BlockInfo[]) => x.concat(y))
              .find((b: BlockInfo) => parsedBlockId == b.id);
            setBlock(block);
          })
          .catch((error) => {
            console.log(error);
            if (error.response?.status == 404) setFound(false);
            if (error.response?.status == 403) setIsForbid(true);
          });
      })
      .catch((error) => {
        if (error.response?.status === 404) setFound(false);
        if (error.response?.status === 403) setIsForbid(true);
      });
  }, [courseId]);

  if (isForbid) return forbid;

  if (!found && block === null) return notFound;

  if (block === undefined) return <CustomBeatLoader />;

  return (
    <>
      <EditBlockDispathcer courseId={parsedCourseId} block={block!} />
    </>
  );
}

export default EditBlock;
