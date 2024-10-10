import { expect, afterEach, beforeAll, afterAll } from "vitest";
import { cleanup } from "@testing-library/react";
import * as matchers from "@testing-library/jest-dom/matchers";
import { server } from "./mocks/server";
import mockedNavigate from "./mocks/useNavigate";

expect.extend(matchers);

beforeAll(() => {
  server.listen();
});

afterEach(() => {
  server.resetHandlers();
  mockedNavigate.mockReset();
  cleanup();
});

afterAll(() => {
  server.close();
});
