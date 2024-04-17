import CoursesSidebar from "../../components/sidebar/coursesSidebar";
import BlockViewContainer from "../../components/blocks/blockViewContainer";
import { Route, Routes, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import BlockDispatcher from "../../components/blocks/blockDispatcher";
import api from "../../config/axios";
import { useDispatch } from "react-redux";
import { useAppSelector } from "../../hooks";
import { CourseHierarchyActionTypes } from "../../redux/reducers/courseHierarchyReducer";
import NotFound from "../../components/general/responses/notFound/notFound";
import Forbid from "../../components/general/responses/forbid/forbid";

function Course() {
  const { courseId } = useParams();

  const hierarchyState = useAppSelector((state) => state.hierarchy);
  const dispatch = useDispatch();
  const [courseFound, setCourseFound] = useState(false);
  const [canVisit, setCanVisit] = useState(true);
  const [currentBlock, setCurrentBlock] = useState();
  const id = Number(courseId);

  useEffect(() => {
    if (isNaN(id)) {
      dispatch({ type: CourseHierarchyActionTypes.FETCH_HIERARCHY_ERROR });
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
        dispatch({
          type: CourseHierarchyActionTypes.FETCH_HIERARCHY_SUCCESS,
          payload: response.data,
        });
        setCourseFound(true);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          dispatch({ type: CourseHierarchyActionTypes.FETCH_HIERARCHY_ERROR });
          setCourseFound(false);
        } else if ("response" in error && error.response.status == 403) {
          dispatch({ type: CourseHierarchyActionTypes.FETCH_HIERARCHY_ERROR });
          setCanVisit(false);
        }
      });
  }, []);

  if (isNaN(id) || (hierarchyState.hierarchy != null && !courseFound))
    return <NotFound>Курс не найден</NotFound>;

  if (!canVisit) return <Forbid>У вас нет доступа к этому курсу</Forbid>;

  return (
    <>
      <CoursesSidebar currentBlock={currentBlock}></CoursesSidebar>
      <BlockViewContainer>
        <Routes>
          <Route
            path=":blockId/"
            element={
              <BlockDispatcher
                hierarchy={hierarchyState.hierarchy}
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
