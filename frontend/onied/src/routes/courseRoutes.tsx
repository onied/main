import Course from "@onied/pages/course/course"
import CreateCourse from "@onied/pages/createCourse/createCourse"
import EditBlock from "@onied/pages/editCourse/EditBlock"
import EditCourseHierarchy from "@onied/pages/editCourse/editHierarchy/editHierarchy"
import EditPreview from "@onied/pages/editCourse/editPreview/editPreview"
import ManageModerators from "@onied/pages/manageModerators/manageModerators"
import Preview from "@onied/pages/preview/preview"

import { Route, Routes } from "react-router-dom"

export default function CourseRoutes() {
    return <Routes>
        <Route path="/course/create" element={<CreateCourse />} />
        <Route path="/course/:courseId/learn/*" element={<Course />} />
        <Route path="/course/:courseId/edit/hierarchy" element={<EditCourseHierarchy />} />
        <Route path="/course/:courseId/edit/:blockId" element={<EditBlock />} />
        <Route path="/course/:courseId/edit" element={<EditPreview />} />
        <Route path="/course/:courseId" element={<Preview />} />
        <Route path="/course/:courseId/manageModerators" element={<ManageModerators />} />
    </Routes>
}