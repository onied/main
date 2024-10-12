import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import EditTasksBlockComponent from "./index";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../../test/helpers/backend";

describe("EditTasksBlockComponent", () => {
  it("renders correctly", async () => {
    // Arrange
    const initial = {
      tasks: [
        { id: 1, title: "string", taskType: 0, maxPoints: 5, isNew: false },
      ],
    };
    server.use(
      http.put(backend("/courses/1/edit/tasks/1"), async ({ request }) => {
        const data: any = await request.json();
        if (data.tasks.length == initial.tasks.length)
          return HttpResponse.json(data);
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/tasks/1/for-edit"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/tasks"]}>
        <EditTasksBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeNull();
    });
  });

  it("reports when forbidden", async () => {
    // Arrange
    const initial = {
      tasks: [
        { id: 1, title: "string", taskType: 0, maxPoints: 5, isNew: false },
      ],
    };
    server.use(
      http.put(backend("/courses/1/edit/tasks/1"), async ({ request }) => {
        const data: any = await request.json();
        if (data.tasks.length == initial.tasks.length)
          return HttpResponse.json(data);
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1/tasks/1/for-edit"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/tasks"]}>
        <EditTasksBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не можете/i)).toBeInTheDocument();
    });
  });

  it("actually saves info", async () => {
    // Arrange
    const initial = {
      tasks: [
        { id: 1, title: "string", taskType: 0, maxPoints: 5, isNew: false },
      ],
    };
    server.use(
      http.put(backend("/courses/1/edit/tasks/1"), async ({ request }) => {
        const data: any = await request.json();
        if (data.tasks.length == initial.tasks.length)
          return HttpResponse.json(data);
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/tasks/1/for-edit"), () => {
        return HttpResponse.json({}, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/tasks"]}>
        <EditTasksBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  test.each([
    /одного ответа/i,
    /многих ответов/i,
    /ручной проверкой/i,
    /текстовым ответом/i,
  ])("edits a task correctly", async (taskType) => {
    // Arrange
    const initial = {
      tasks: [
        { id: 1, title: "string", taskType: 0, maxPoints: 5, isNew: false },
      ],
    };
    server.use(
      http.put(backend("/courses/1/edit/tasks/1"), async ({ request }) => {
        const data: any = await request.json();
        console.error(data);
        if (
          data.tasks.length == initial.tasks.length &&
          data.tasks[0].title == "stringasdfasdf" &&
          data.tasks[0].maxPoints == 51
        )
          return HttpResponse.json(data);
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/tasks/1/for-edit"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/tasks"]}>
        <EditTasksBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const taskTitle = await screen.findByLabelText(/текст задания/i);
    await user.type(taskTitle, "asdfasdf");
    const maxPoints = await screen.findByLabelText(/количество баллов/i);
    await user.type(maxPoints, "1");
    const taskTypeSelect = await screen.findByLabelText(/тип задания/i);
    await user.selectOptions(taskTypeSelect, await screen.findByText(taskType));

    const button = await screen.findByText(/сохранить изменения/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(taskTitle).toHaveTextContent("stringasdfasdf");
      expect(maxPoints).toHaveValue(51);
    });
  });
  it("adds tasks correctly", async () => {
    // Arrange
    const initial = {
      tasks: [],
    };
    server.use(
      http.put(backend("/courses/1/edit/tasks/1"), async ({ request }) => {
        const data: any = await request.json();
        console.error(data);
        if (
          data.tasks.length == 1 &&
          data.tasks[0].title == "asdfasdf" &&
          data.tasks[0].maxPoints == 1
        )
          return HttpResponse.json(data);
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/tasks/1/for-edit"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/tasks"]}>
        <EditTasksBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const addButton = await screen.findByText(/добавить задание/i);
    await user.click(addButton);

    const taskTitle = await screen.findByLabelText(/текст задания/i);
    await user.type(taskTitle, "asdfasdf");
    const maxPoints = await screen.findByLabelText(/количество баллов/i);
    await user.type(maxPoints, "1");

    const button = await screen.findByText(/сохранить изменения/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(taskTitle).toHaveTextContent("asdfasdf");
      expect(maxPoints).toHaveValue(1);
    });
  });
  it("deletes tasks correctly", async () => {
    // Arrange
    const initial = {
      tasks: [
        { id: 1, title: "string", taskType: 0, maxPoints: 5, isNew: false },
      ],
    };
    server.use(
      http.put(backend("/courses/1/edit/tasks/1"), async ({ request }) => {
        const data: any = await request.json();
        console.error(data);
        if (data.tasks.length == 0) return HttpResponse.json(data);
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/tasks/1/for-edit"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/tasks"]}>
        <EditTasksBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const trashButton = await screen.findByTestId("trash");
    await user.click(trashButton);

    const button = await screen.findByText(/сохранить изменения/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(trashButton).not.toBeInTheDocument();
    });
  });
  it("handles NaNs as course and block id", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditTasksBlockComponent courseId={NaN} blockId={NaN} />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
});
