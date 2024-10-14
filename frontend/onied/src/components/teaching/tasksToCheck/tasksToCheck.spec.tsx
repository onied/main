import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import TasksToCheck from "./tasksToCheck";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../test/helpers/backend";
import { MemoryRouter } from "react-router-dom";

describe("TasksToCheck", () => {
  it("renders correctly", async () => {
    // Arrange
    const initial = [
      {
        courseId: 1,
        title: "Course title",
        tasksToCheck: [
          {
            index: "ee631be0-aa5f-48c2-b975-f9ba328aa83a",
            moduleTitle: "Module title",
            blockTitle: "Block title",
            title: "Task title",
          },
        ],
      },
    ];
    server.use(
      http.get(backend("/teaching/tasks-to-check-list"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter>
        <TasksToCheck />
      </MemoryRouter>
    );

    // Act
    const link = await screen.findByText(/проверить/i);

    // Assert
    await waitFor(() => {
      expect(link).toHaveAttribute(
        "href",
        "/" + initial[0].tasksToCheck[0].index
      );
    });
  });
  it("reports no access", async () => {
    // Arrange
    server.use(
      http.get(backend("/teaching/tasks-to-check-list"), () => {
        return HttpResponse.json({}, { status: 403 });
      })
    );
    render(
      <MemoryRouter>
        <TasksToCheck />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нет прав/i)).toBeInTheDocument();
    });
  });
  it("reports no tasks to check", async () => {
    // Arrange
    server.use(
      http.get(backend("/teaching/tasks-to-check-list"), () => {
        return HttpResponse.json([]);
      })
    );
    render(
      <MemoryRouter>
        <TasksToCheck />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нет ответов на задания/i)).toBeInTheDocument();
    });
  });
});
