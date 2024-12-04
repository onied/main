import Course from "@onied/pages/course/course";
import CreateCourse from "@onied/pages/createCourse/createCourse";
import EditBlock from "@onied/pages/editCourse/EditBlock";
import EditCourseHierarchy from "@onied/pages/editCourse/editHierarchy/editHierarchy";
import EditPreview from "@onied/pages/editCourse/editPreview/editPreview";
import ManageModerators from "@onied/pages/manageModerators/manageModerators";
import Preview from "@onied/pages/preview/preview";

import { Route, Routes } from "react-router-dom";

export default function CourseRoutes() {
  return (
    <Routes>
      <Route path="/create" element={<CreateCourse />} />
      <Route path="/:courseId/learn/*" element={<Course />} />
      <Route
        path="/:courseId/edit/hierarchy"
        element={<EditCourseHierarchy />}
      />
      <Route path="/:courseId/edit/:blockId" element={<EditBlock />} />
      <Route path="/:courseId/edit" element={<EditPreview />} />
      <Route path="/:courseId" element={<Preview />} />
      <Route
        path="/:courseId/manageModerators"
        element={<ManageModerators />}
      />
    </Routes>
  );
}
