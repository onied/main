import "@testing-library/jest-dom";
import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import EditCourseHierarchy from "./editHierarchy";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../test/helpers/backend";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { BlockType } from "../../../types/block";

describe("EditCourseHierarchy", () => {
  it("renders correctly", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByPlaceholderText(/название блока/i)).toHaveValue(
        initial.modules[0].blocks[0].title
      );
      expect(screen.queryByPlaceholderText(/название модуля/i)).toHaveValue(
        initial.modules[0].title
      );
    });
  });
  it("expands module", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const expand = await screen.findByLabelText("раскрыть");
    await user.click(expand);

    // Assert
    await waitFor(() => {
      expect(screen.queryByLabelText("раскрыть")).toBeNull();
      expect(screen.queryByLabelText("скрыть")).toBeInTheDocument();
      expect(
        screen.queryAllByPlaceholderText(/название модуля/i)[0]
      ).toHaveValue(initial.modules[0].title);
    });
  });
  it("shrinks module", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const expand = await screen.findByLabelText("раскрыть");
    await user.click(expand);
    const shrink = await screen.findByLabelText("скрыть");
    await user.click(shrink);

    // Assert
    await waitFor(() => {
      expect(screen.queryByLabelText("скрыть")).toBeNull();
      expect(screen.queryByLabelText("раскрыть")).toBeInTheDocument();
      expect(
        screen.queryAllByPlaceholderText(/название модуля/i)[0]
      ).toHaveValue(initial.modules[0].title);
    });
  });
  it("adds module", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.post(backend("courses/2/edit/add-module"), () => {
        return HttpResponse.json(2);
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const addModule = await screen.findByText(/добавить модуль/i);
    await user.click(addModule);

    // Assert
    await waitFor(() => {
      expect(screen.queryByPlaceholderText(/название блока/i)).toHaveValue(
        initial.modules[0].blocks[0].title
      );
      expect(
        screen.queryAllByPlaceholderText(/название модуля/i)[0]
      ).toHaveValue(initial.modules[0].title);
      expect(
        screen.queryAllByPlaceholderText(/название модуля/i)[1]
      ).toHaveValue("Новый модуль");
    });
  });
  it("renames module", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.put(backend("courses/2/edit/rename-module"), async ({ request }) => {
        const data: any = await request.json();
        if (data.moduleId == 1 && data.title == "Module title for real")
          return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const renameModule = await screen.findByPlaceholderText(/название модуля/i);
    await user.type(renameModule, " for real");
    await user.tab();

    // Assert
    await waitFor(() => {
      expect(screen.queryByPlaceholderText(/название блока/i)).toHaveValue(
        initial.modules[0].blocks[0].title
      );
      expect(screen.queryByPlaceholderText(/название модуля/i)).toHaveValue(
        "Module title for real"
      );
    });
  });
  it("deletes module", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.delete(
        backend("courses/2/edit/delete-module"),
        async ({ request }) => {
          const data = new URL(request.url).searchParams;
          if (data.get("moduleId") == "1") return HttpResponse.json();
          return HttpResponse.json({}, { status: 404 });
        }
      ),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const deleteModule = await screen.findByLabelText(/удалить модуль/i);
    await user.click(deleteModule);

    // Assert
    await waitFor(() => {
      expect(screen.queryByPlaceholderText(/название блока/i)).toBeNull();
      expect(screen.queryByPlaceholderText(/название модуля/i)).toBeNull();
      expect(screen.queryByText(/не найден/i)).toBeNull();
    });
  });
  test.each([
    [1, /конспект/i],
    [2, /видео/i],
    [3, /задания/i],
  ])("adds block", async (blockType, blockTypeText) => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.post(backend("courses/2/edit/add-block/1"), async ({ request }) => {
        const data: any = await request.json();
        if (data.blockType == blockType) return HttpResponse.json(1);
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const addBlockButton = await screen.findByLabelText(/добавить блок/i);
    await user.click(addBlockButton);
    const addBlock = await screen.findByText(blockTypeText);
    await user.click(addBlock);

    // Assert
    await waitFor(() => {
      expect(screen.queryByLabelText(blockTypeText)).toBeInTheDocument();
      expect(
        screen.queryAllByPlaceholderText(/название блока/i)[0]
      ).toHaveValue("Новый блок");
    });
  });
  it("renames block", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.put(backend("courses/2/edit/rename-block"), async ({ request }) => {
        const data: any = await request.json();
        if (data.blockId == 1 && data.title == "Block title for real")
          return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const renameBlock = await screen.findByPlaceholderText(/название блока/i);
    await user.type(renameBlock, " for real");
    await user.tab();

    // Assert
    await waitFor(() => {
      expect(screen.queryByPlaceholderText(/название блока/i)).toHaveValue(
        "Block title for real"
      );
      expect(screen.queryByPlaceholderText(/название модуля/i)).toHaveValue(
        initial.modules[0].title
      );
    });
  });
  it("deletes block", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.delete(
        backend("courses/2/edit/delete-block"),
        async ({ request }) => {
          const data = new URL(request.url).searchParams;
          if (data.get("blockId") == "1") return HttpResponse.json();
          return HttpResponse.json({}, { status: 404 });
        }
      ),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const deleteModule = await screen.findByLabelText(/удалить блок/i);
    await user.click(deleteModule);

    // Assert
    await waitFor(() => {
      expect(screen.queryByPlaceholderText(/название блока/i)).toBeNull();
      expect(
        screen.queryByPlaceholderText(/название модуля/i)
      ).toBeInTheDocument();
      expect(screen.queryByText(/не найден/i)).toBeNull();
    });
  });
  it("rearranges blocks between modules", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title 1",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title 1",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
        {
          id: 2,
          index: 0,
          title: "Module title 2",
          blocks: [
            {
              id: 2,
              index: 0,
              title: "Block title 2",
              blockType: BlockType.VideoBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.put(backend("courses/2/edit/hierarchy"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act
    const blocks = await screen.findAllByLabelText("блок");
    fireEvent.dragStart(blocks[1]);
    fireEvent.dragEnter(blocks[0]);
    fireEvent.drop(blocks[0]);
    fireEvent.dragLeave(blocks[0]);
    fireEvent.dragEnd(blocks[1]);

    // Assert
    await waitFor(() => {
      expect(screen.queryAllByPlaceholderText("название блока")[0]).toHaveValue(
        initial.modules[1].blocks[0].title
      );
      expect(screen.queryAllByPlaceholderText("название блока")[1]).toHaveValue(
        initial.modules[0].blocks[0].title
      );
    });
  });
  it("rearranges modules with blocks", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title 1",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title 1",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
        {
          id: 2,
          index: 0,
          title: "Module title 2",
          blocks: [
            {
              id: 2,
              index: 0,
              title: "Block title 2",
              blockType: BlockType.VideoBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.put(backend("courses/2/edit/hierarchy"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act
    const modules = await screen.findAllByLabelText("модуль");
    fireEvent.dragStart(modules[1]);
    fireEvent.dragEnter(modules[0]);
    fireEvent.drop(modules[0]);
    fireEvent.dragLeave(modules[0]);
    fireEvent.dragEnd(modules[1]);

    // Assert
    await waitFor(() => {
      expect(
        screen.queryAllByPlaceholderText("название модуля")[0]
      ).toHaveValue(initial.modules[1].title);
      expect(
        screen.queryAllByPlaceholderText("название модуля")[1]
      ).toHaveValue(initial.modules[0].title);
      expect(screen.queryAllByPlaceholderText("название блока")[0]).toHaveValue(
        initial.modules[1].blocks[0].title
      );
      expect(screen.queryAllByPlaceholderText("название блока")[1]).toHaveValue(
        initial.modules[0].blocks[0].title
      );
    });
  });
  it("handles invalid course id", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/asdf" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports not found", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports not found from other endpoint", async () => {
    // Arrange
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json({}, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports not found while editing", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.post(backend("courses/2/edit/add-module"), () => {
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const addModule = await screen.findByText(/добавить модуль/i);
    await user.click(addModule);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports forbidden", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не можете/i)).toBeInTheDocument();
    });
  });
  it("reports forbidden from other endpoint", async () => {
    // Arrange
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json({}, { status: 403 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не можете/i)).toBeInTheDocument();
    });
  });
  it("reports forbidden while editing", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.post(backend("courses/2/edit/add-module"), () => {
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const addModule = await screen.findByText(/добавить модуль/i);
    await user.click(addModule);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не можете/i)).toBeInTheDocument();
    });
  });
  it("rearranges blocks inside module with keyboard", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title 1",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title 1",
              blockType: BlockType.SummaryBlock,
            },
            {
              id: 2,
              index: 0,
              title: "Block title 2",
              blockType: BlockType.VideoBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.put(backend("courses/2/edit/hierarchy"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act
    const blocks = await screen.findAllByLabelText("блок");
    fireEvent.keyPress(blocks[0], { keyCode: 32 });
    fireEvent.keyPress(blocks[0], { keyCode: 40 });
    fireEvent.keyPress(blocks[0], { keyCode: 32 });

    // Assert
    await waitFor(() => {
      expect(screen.queryAllByPlaceholderText("название блока")[0]).toHaveValue(
        initial.modules[0].blocks[1].title
      );
      expect(screen.queryAllByPlaceholderText("название блока")[1]).toHaveValue(
        initial.modules[0].blocks[0].title
      );
    });
  });
  it("rearranges modules with blocks with keyboard", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          id: 1,
          index: 0,
          title: "Module title 1",
          blocks: [
            {
              id: 1,
              index: 0,
              title: "Block title 1",
              blockType: BlockType.SummaryBlock,
            },
          ],
        },
        {
          id: 2,
          index: 0,
          title: "Module title 2",
          blocks: [
            {
              id: 2,
              index: 0,
              title: "Block title 2",
              blockType: BlockType.VideoBlock,
            },
          ],
        },
      ],
    };
    server.use(
      http.get(backend("courses/2/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.put(backend("courses/2/edit/hierarchy"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/edit/2" }]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditCourseHierarchy />} />
        </Routes>
      </MemoryRouter>
    );

    // Act
    const modules = await screen.findAllByLabelText("модуль");
    fireEvent.keyPress(modules[0], { keyCode: 32 });
    fireEvent.keyPress(modules[0], { keyCode: 40 });
    fireEvent.keyPress(modules[0], { keyCode: 32 });

    // Assert
    await waitFor(() => {
      expect(
        screen.queryAllByPlaceholderText("название модуля")[0]
      ).toHaveValue(initial.modules[1].title);
      expect(
        screen.queryAllByPlaceholderText("название модуля")[1]
      ).toHaveValue(initial.modules[0].title);
      expect(screen.queryAllByPlaceholderText("название блока")[0]).toHaveValue(
        initial.modules[1].blocks[0].title
      );
      expect(screen.queryAllByPlaceholderText("название блока")[1]).toHaveValue(
        initial.modules[0].blocks[0].title
      );
    });
  });
});
