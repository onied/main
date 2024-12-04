import CheckTask from "@onied/pages/checkTasks/checkTask/checkTask";
import TeachingPage from "@onied/pages/teaching/teaching";

import { Route, Routes } from "react-router-dom";

export default function TeachingRoutes() {
  return (
    <Routes>
      <Route path="/check/:taskCheckId" element={<CheckTask />} />
      <Route path="/*" element={<TeachingPage />} />
    </Routes>
  );
}
