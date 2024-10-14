import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import EditBlock from "./EditBlock";
import { http, HttpResponse } from "msw";
import { server } from "../../../test/mocks/server";
import { describe, it, expect, vi } from "vitest";
import backend from "../../../test/helpers/backend";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { BlockType } from "../../types/block";
import * as EditSummaryBlock from "../../components/editCourse/editBlocks/editSummaryBlock/editSummaryBlock";
import * as EditVideoBlock from "../../components/editCourse/editBlocks/editVideoBlock/editVideoBlock";
import * as EditTasksBlock from "../../components/editCourse/editBlocks/editTasksBlock/index";

describe("EditBlock", () => {
  it("renders summary block correctly", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          blocks: [
            {
              id: 1,
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
    const mockedEditBlock = vi.fn();
    vi.spyOn(EditSummaryBlock, "default").mockImplementation(mockedEditBlock);
    render(
      <MemoryRouter initialEntries={["/edit/2/1"]}>
        <Routes>
          <Route path="/edit/:courseId/:blockId" element={<EditBlock />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(mockedEditBlock).toHaveBeenCalledWith(
        expect.objectContaining({
          blockId: 1,
          courseId: 2,
        }),
        expect.anything()
      );
    });
  });
  it("renders video block correctly", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          blocks: [
            {
              id: 1,
              title: "Block title",
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
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    const mockedEditBlock = vi.fn();
    vi.spyOn(EditVideoBlock, "default").mockImplementation(mockedEditBlock);
    render(
      <MemoryRouter initialEntries={["/edit/2/1"]}>
        <Routes>
          <Route path="/edit/:courseId/:blockId" element={<EditBlock />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(mockedEditBlock).toHaveBeenCalledWith(
        expect.objectContaining({
          blockId: 1,
          courseId: 2,
        }),
        expect.anything()
      );
    });
  });
  it("renders task block correctly", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          blocks: [
            {
              id: 1,
              title: "Block title",
              blockType: BlockType.TasksBlock,
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
    const mockedEditBlock = vi.fn();
    vi.spyOn(EditTasksBlock, "default").mockImplementation(mockedEditBlock);
    render(
      <MemoryRouter initialEntries={["/edit/2/1"]}>
        <Routes>
          <Route path="/edit/:courseId/:blockId" element={<EditBlock />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(mockedEditBlock).toHaveBeenCalledWith(
        expect.objectContaining({
          blockId: 1,
          courseId: 2,
        }),
        expect.anything()
      );
    });
  });
  it("handles invalid courseId and blockId", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          blocks: [
            {
              id: 1,
              title: "Block title",
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
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/asdfasdf/asdfasdf"]}>
        <Routes>
          <Route path="/edit/:courseId/:blockId" element={<EditBlock />} />
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
          blocks: [
            {
              id: 1,
              title: "Block title",
              blockType: BlockType.VideoBlock,
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
      <MemoryRouter initialEntries={["/edit/2/1"]}>
        <Routes>
          <Route path="/edit/:courseId/:blockId" element={<EditBlock />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

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
          blocks: [
            {
              id: 1,
              title: "Block title",
              blockType: BlockType.VideoBlock,
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
      <MemoryRouter initialEntries={["/edit/2/1"]}>
        <Routes>
          <Route path="/edit/:courseId/:blockId" element={<EditBlock />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не можете/i)).toBeInTheDocument();
    });
  });
  it("reports when backend returns not found", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          blocks: [
            {
              id: 1,
              title: "Block title",
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
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/2/1"]}>
        <Routes>
          <Route path="/edit/:courseId/:blockId" element={<EditBlock />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports when backend returns forbidden", async () => {
    // Arrange
    const initial = {
      modules: [
        {
          blocks: [
            {
              id: 1,
              title: "Block title",
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
      http.get(backend("/courses/2/hierarchy"), () => {
        return HttpResponse.json(initial, { status: 403 });
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/2/1"]}>
        <Routes>
          <Route path="/edit/:courseId/:blockId" element={<EditBlock />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не можете/i)).toBeInTheDocument();
    });
  });
});
