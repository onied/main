import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import CheckTaskComponent from "./checkTask";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect, vi } from "vitest";
import backend from "../../../../test/helpers/backend";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import mockedNavigate from "../../../../test/mocks/useNavigate";

describe("CheckTaskComponent", () => {
  it("renders correctly", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), () => {
        return HttpResponse.json({}, { status: 400 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter
        initialEntries={[{ pathname: "/check/" + encodeURIComponent(taskId) }]}
      >
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(
        screen.queryByText(initial.task.block.module.course.title)
      ).toBeInTheDocument();
      expect(
        screen.queryByText(initial.task.block.module.title)
      ).toBeInTheDocument();
      expect(screen.queryByText(initial.task.block.title)).toBeInTheDocument();
      expect(screen.queryByText(initial.task.title)).toBeInTheDocument();
      expect(screen.queryByText(initial.task.title)).toBeInTheDocument();
      expect(screen.queryByText(/непроверено/i)).toBeInTheDocument();
    });
  });
  it("reports not found", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), () => {
        return HttpResponse.json(initial, { status: 404 });
      }),
      http.put(backend("/teaching/check/" + taskId), () => {
        return HttpResponse.json({}, { status: 400 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter
        initialEntries={[{ pathname: "/check/" + encodeURIComponent(taskId) }]}
      >
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не.* найден/i)).toBeInTheDocument();
    });
  });
  it("reports no access", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), () => {
        return HttpResponse.json(initial, { status: 403 });
      }),
      http.put(backend("/teaching/check/" + taskId), () => {
        return HttpResponse.json({}, { status: 400 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter
        initialEntries={[{ pathname: "/check/" + encodeURIComponent(taskId) }]}
      >
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нет прав/i)).toBeInTheDocument();
    });
  });
  it("returns after saving points", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), () => {
        return HttpResponse.json();
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/check/" + taskId }]}>
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/сохранить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(-1);
    });
  });
  it("saves points", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), async ({ request }) => {
        const data: any = await request.json();
        if (data.points == 1) return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/check/" + taskId }]}>
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const input = await screen.findByLabelText(/балл/i);
    await user.type(input, "1");
    const button = await screen.findByText(/сохранить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(-1);
    });
  });
  it("increases points when clicked on button", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), async ({ request }) => {
        const data: any = await request.json();
        if (data.points == 1) return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/check/" + taskId }]}>
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const buttonIncrease = await screen.findByTestId("increasePoints");
    await user.click(buttonIncrease);
    const button = await screen.findByText(/сохранить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(-1);
    });
  });
  it("decreases points when clicked on button", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: true,
      points: 1,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), async ({ request }) => {
        const data: any = await request.json();
        if (data.points == 0) return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/check/" + taskId }]}>
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const buttonDecrease = await screen.findByTestId("decreasePoints");
    await user.click(buttonDecrease);
    const button = await screen.findByText(/сохранить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(-1);
    });
  });
  it("does not increase more than max points", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), async ({ request }) => {
        const data: any = await request.json();
        if (data.points == 1) return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/check/" + taskId }]}>
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const buttonIncrease = await screen.findByTestId("increasePoints");
    await user.click(buttonIncrease);
    await user.click(buttonIncrease);
    await user.click(buttonIncrease);
    await user.click(buttonIncrease);
    await user.click(buttonIncrease);
    await user.click(buttonIncrease);
    const button = await screen.findByText(/сохранить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(-1);
    });
  });
  it("does not decrease less than zero", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: true,
      points: 1,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), async ({ request }) => {
        const data: any = await request.json();
        if (data.points == 0) return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/check/" + taskId }]}>
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const buttonDecrease = await screen.findByTestId("decreasePoints");
    await user.click(buttonDecrease);
    await user.click(buttonDecrease);
    await user.click(buttonDecrease);
    await user.click(buttonDecrease);
    await user.click(buttonDecrease);
    await user.click(buttonDecrease);
    const button = await screen.findByText(/сохранить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(-1);
    });
  });
  it("reports invalid value", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json({ errors: { Points: true } }, { status: 400 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/check/" + taskId }]}>
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const input = await screen.findByLabelText(/балл/i);
    await user.type(input, "1");
    const button = await screen.findByText(/сохранить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/некоррект/i)).toBeInTheDocument();
    });
  });
  it("reports forbidden while checking", async () => {
    // Arrange
    const taskId = "55c76724-ce7f-428b-a703-9cdecfd82142";
    const initial = {
      task: {
        block: {
          module: {
            course: {
              title: "Course title",
            },
            index: 1,
            title: "Module title",
          },
          index: 1,
          title: "Block title",
        },
        title: "Task title",
        maxPoints: 1,
      },
      content: "asdfasdfasdfasdfasdfasdfasdf",
      checked: false,
      points: 0,
    };
    server.use(
      http.get(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json(initial);
      }),
      http.put(backend("/teaching/check/" + taskId), async () => {
        return HttpResponse.json({}, { status: 403 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/check/" + taskId }]}>
        <Routes>
          <Route path="/check/:taskCheckId" element={<CheckTaskComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const input = await screen.findByLabelText(/балл/i);
    await user.type(input, "1");
    const button = await screen.findByText(/сохранить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нет прав/i)).toBeInTheDocument();
    });
  });
});
