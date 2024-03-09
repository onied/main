import Sidebar from "../../components/sidebar/sidebar";
import BlockViewContainer from "../../components/blocks/blockViewContainer";
import Summary from "../../components/blocks/summary/summary";
import Tasks from "../../components/blocks/tasks/tasks";
import EmbedVideo from "../../components/blocks/video/embedVideo";
import { Route, Routes, useParams } from "react-router-dom";
import Config from "../../config/config";
import axios from "axios";
import { useEffect, useState } from "react";

function Course() {
  const { courseId } = useParams();
  const [hierarchy, setHierarchy] = useState();

  const id = Number(courseId);
  if (isNaN(id)) {
    console.log(id);
    console.log(courseId);
    return <h1>Курс не найден.</h1>;
  }

  useEffect(() => {
    axios
      .get(Config.CoursesBackend + "courses/" + id + "/get_hierarchy")
      .then((response) => {
        console.log(response.data);
        setHierarchy(response.data);
      })
      .catch((error) => console.log(error));
  }, []);

  return (
    <>
      <Sidebar hierarchy={hierarchy}></Sidebar>
      <BlockViewContainer>
        <Routes>
          <Route path="1" element={Summary()}></Route>
          <Route path="2" element={Tasks()}></Route>
          <Route
            path="3"
            element={
              <>
                <EmbedVideo href="https://www.youtube.com/watch?v=YfBlwC44gDQ" />
                <EmbedVideo href="https://vk.com/video-50883936_456243146" />
                <EmbedVideo href="https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd" />
              </>
            }
          ></Route>
        </Routes>
      </BlockViewContainer>
    </>
  );
}
export default Course;
