import "./App.css";
import Header from "./components/header/header";
import Course from "./pages/course/course";
import { Route, Routes } from "react-router-dom";
import Preview from "./pages/preview/preview";
import Catalog from "./pages/catalog/catalog.jsx";
import Register from "./pages/accountManagement/register/register.jsx";
import Login from "./pages/accountManagement/login/login";
import TwoFactor from "./pages/accountManagement/twoFactorAuth/twoFactor.jsx";

function App() {
  return (
    <>
      <Header></Header>
      <main>
        <Routes>
          <Route path="/course/:courseId/learn/*" element={<Course />}></Route>
          <Route path="/course/:courseId" element={<Preview />}></Route>
          <Route path="/catalog" element={<Catalog />}></Route>
          <Route path="/register" element={<Register />}></Route>
          <Route path="/login" element={<Login />}></Route>
          <Route path="/two-factor" element={<TwoFactor />}></Route>s
        </Routes>
      </main>
    </>
  );
}

export default App;
