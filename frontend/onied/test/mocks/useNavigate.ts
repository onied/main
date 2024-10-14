import { vitest } from "vitest";

const mockedNavigate = vitest.fn();

vitest.mock("react-router-dom", async (orig) => {
  return {
    ...(await orig()),
    useNavigate: () => mockedNavigate,
  };
});

export default mockedNavigate;
