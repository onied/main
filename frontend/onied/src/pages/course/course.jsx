import Sidebar from "../../components/sidebar/sidebar";
import BlockViewContainer from "../../components/blocks/blockViewContainer";
import { Route, Routes, useParams } from "react-router-dom";
import Config from "../../config/config";
import axios from "axios";
import { useEffect, useState } from "react";
import BlockDispatcher from "../../components/blocks/blockDispatcher";

function Course() {
  const { courseId } = useParams();
  const [hierarchy, setHierarchy] = useState();
  const [currentBlock, setCurrentBlock] = useState();

  const id = Number(courseId);
  if (isNaN(id)) {
    console.log(id);
    console.log(courseId);
    return <h1>Курс не найден.</h1>;
  }

  useEffect(() => {
    axios
      .get(Config.CoursesBackend + "courses/" + id + "/get_hierarchy/")
      .then((response) => {
        console.log(response.data);
        setHierarchy(response.data);
        if (
          "modules" in response.data &&
          response.data.modules.length > 0 &&
          "blocks" in response.data.modules[0] &&
          response.data.modules[0].blocks.length > 0
        )
          setCurrentBlock(response.data.modules[0].blocks[0].id);
      })
      .catch((error) => console.log(error));
  }, []);

  return (
    <>
      <Sidebar
        hierarchy={hierarchy}
        currentBlock={currentBlock}
        setCurrentBlock={setCurrentBlock}
      ></Sidebar>
      <BlockViewContainer>
        <Routes>
          <Route
            path=":blockId/"
            element={<BlockDispatcher hierarchy={hierarchy} />}
          />
        </Routes>
      </BlockViewContainer>
    </>
  );
}
export default Course;
