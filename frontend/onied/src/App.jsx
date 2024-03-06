import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";
import Header from "./components/header/header";
import Sidebar from "./components/sidebar/sidebar";

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <Header></Header>
      <main>
        <Sidebar></Sidebar>
      </main>
    </>
  );
}

export default App;
