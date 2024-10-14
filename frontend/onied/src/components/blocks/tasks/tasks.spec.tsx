import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { describe, it, expect, vi } from "vitest";

import { server } from "@onied/tests/mocks/server";
import backend from "@onied/tests/helpers/backend";
import { BlockType, TasksBlock } from "@onied/types/block";
import Tasks from "./tasks";
import * as hooks from "@onied/hooks";
import { TaskType } from "../../../types/task";

describe("Tasks", () => {
  it("renders tasks with no file when valid response", async () => {
    const courseId = 1;
    const blockId = 1;
    const url = `courses/${courseId}/tasks/${blockId}`;

    const tasks: TasksBlock = {
      id: blockId,
      title: "default text",
      blockType: BlockType.TasksBlock,
      completed: false,
      tasks: [
        {
          id: 1,
          title: "string",
          taskType: TaskType.ManualReview,
          maxPoints: 5,
          isNew: false,
          variants: undefined,
        },
      ],
    };

    server.use(
      http.get(backend(url), () => {
        return HttpResponse.json(tasks, { status: 200 });
      }),
      http.get(backend("courses/1/tasks/1/points"), () => {
        return HttpResponse.json([]);
      })
    );

    vi.spyOn(hooks, "useAppSelector").mockImplementation((_) => {
      return { hierarchy: { modules: [{ blocks: [{}] }] } };
    });

    render(
      <MemoryRouter initialEntries={[url]}>
        <Tasks courseId={courseId} blockId={blockId} />
      </MemoryRouter>
    );

    await waitFor(() => {
      expect(screen.queryByText("string")).toBeInTheDocument();
    });
  });
});
