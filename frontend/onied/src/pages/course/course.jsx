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
  const [courseFound, setCourseFound] = useState(false);
  const [currentBlock, setCurrentBlock] = useState();
  const notFound = <h1 style={{ margin: "3rem" }}>Курс не найден.</h1>;

  const id = Number(courseId);
  if (isNaN(id)) {
    console.log(id);
    console.log(courseId);
    return notFound;
  }

  useEffect(() => {
    axios
      .get(Config.CoursesBackend + "courses/" + id + "/get_hierarchy/")
      .then((response) => {
        console.log(response.data);
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

  if (hierarchy != null && !courseFound) return notFound;

  return (
    <>
      <Sidebar hierarchy={hierarchy} currentBlock={currentBlock}></Sidebar>
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
            element={
              <h1 style={{ margin: "3rem" }}>Выберите блок из списка.</h1>
            }
          />
        </Routes>
      </BlockViewContainer>
    </>
  );
}
export default Course;
