import CoursesSidebar from "../../components/sidebar/coursesSidebar";
import BlockViewContainer from "../../components/blocks/blockViewContainer";
import { Route, Routes, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import BlockDispatcher from "../../components/blocks/blockDispatcher";
import api from "../../config/axios";
import NotFound from "../../components/general/responses/notFound/notFound";

function Course() {
  const { courseId } = useParams();
  const [hierarchy, setHierarchy] = useState();
  const [courseFound, setCourseFound] = useState(false);
  const [currentBlock, setCurrentBlock] = useState();
  const notFound = <NotFound>Курс не найден.</NotFound>;
  const id = Number(courseId);

  useEffect(() => {
    if (isNaN(id)) {
      setHierarchy({});
      setCourseFound(false);
      return;
    }
    api
      .get("courses/" + id + "/hierarchy/")
      .then((response) => {
        console.log(response.data);
        if ("modules" in response.data) {
          response.data.modules.sort((a, b) => (a.index > b.index ? 1 : -1));
          response.data.modules.forEach((module) => {
            if ("blocks" in module)
              module.blocks.sort((a, b) => (a.index > b.index ? 1 : -1));
          });
        }
        setHierarchy(response.data);
        setCourseFound(true);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setHierarchy({});
          setCourseFound(false);
        }
      });
  }, []);

  if (isNaN(id)) {
    console.log(id);
    console.log(courseId);
    return notFound;
  }

  if (hierarchy != null && !courseFound) return notFound;

  return (
    <>
      <CoursesSidebar
        hierarchy={hierarchy}
        currentBlock={currentBlock}
      ></CoursesSidebar>
      <BlockViewContainer>
        <Routes>
          <Route
            path=":blockId/"
            element={
              <BlockDispatcher
                hierarchy={hierarchy}
                setCurrentBlock={setCurrentBlock}
              />
            }
          />
          <Route
            path="*"
            element={<NotFound>Выберите блок из списка.</NotFound>}
          />
        </Routes>
      </BlockViewContainer>
    </>
  );
}
export default Course;
