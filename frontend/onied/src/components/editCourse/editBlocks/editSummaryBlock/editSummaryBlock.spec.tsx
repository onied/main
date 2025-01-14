import "@testing-library/jest-dom";
import { render, screen, waitFor, fireEvent } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import EditSummaryBlockComponent from "./editSummaryBlock";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../../test/helpers/backend";

describe("EditSummaryBlockComponent", () => {
  it("renders correctly", async () => {
    // Arrange
    const initial = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
    };
    const expected = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
      fileName: "string",
      fileHref: "string",
    };
    server.use(
      http.put(backend("/courses/1/edit/summary/1"), async ({ request }) => {
        const data: any = await request.json();
        if (
          expected.id == data.id &&
          expected.title == data.title &&
          expected.blockType == data.blockType &&
          expected.isCompleted == data.isCompleted &&
          expected.markdownText == data.markdownText &&
          expected.fileName == data.fileName &&
          expected.fileHref == data.fileHref
        )
          return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/summary/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditSummaryBlockComponent courseId={1} blockId={1} />
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
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
      fileName: "string",
      fileHref: "string",
    };
    const expected = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
      fileName: "string",
      fileHref: "string",
    };
    server.use(
      http.put(backend("/courses/1/edit/summary/1"), async ({ request }) => {
        const data: any = await request.json();
        if (
          expected.id == data.id &&
          expected.title == data.title &&
          expected.blockType == data.blockType &&
          expected.isCompleted == data.isCompleted &&
          expected.markdownText == data.markdownText &&
          expected.fileName == data.fileName &&
          expected.fileHref == data.fileHref
        )
          return HttpResponse.json();
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1/summary/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditSummaryBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не можете/i)).toBeInTheDocument();
    });
  });
  it("reports when not found", async () => {
    // Arrange
    const expected = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
      fileName: "string",
      fileHref: "string",
    };
    server.use(
      http.put(backend("/courses/1/edit/summary/1"), async ({ request }) => {
        const data: any = await request.json();
        if (
          expected.id == data.id &&
          expected.title == data.title &&
          expected.blockType == data.blockType &&
          expected.isCompleted == data.isCompleted &&
          expected.markdownText == data.markdownText &&
          expected.fileName == data.fileName &&
          expected.fileHref == data.fileHref
        )
          return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/summary/1"), () => {
        return HttpResponse.json({}, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditSummaryBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports when not found while saving", async () => {
    // Arrange
    const initial = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
      fileName: "string",
      fileHref: "string",
    };
    server.use(
      http.put(backend("/courses/1/edit/summary/1"), () => {
        // const data: any = await request.json();
        // if (
        //   expected.id == data.id &&
        //   expected.title == data.title &&
        //   expected.blockType == data.blockType &&
        //   expected.isCompleted == data.isCompleted &&
        //   expected.markdownText == data.markdownText &&
        //   expected.fileName == data.fileName &&
        //   expected.fileHref == data.fileHref
        // )
        //   return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/summary/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditSummaryBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports when forbidden while saving", async () => {
    // Arrange
    const initial = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
      fileName: "string",
      fileHref: "string",
    };
    server.use(
      http.put(backend("/courses/1/edit/summary/1"), () => {
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/summary/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditSummaryBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не можете/i)).toBeInTheDocument();
    });
  });
  it("actually saves info", async () => {
    // Arrange
    const initial = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
    };
    const expected = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "stringasdf",
    };
    server.use(
      http.put(backend("/courses/1/edit/summary/1"), async ({ request }) => {
        const data: any = await request.json();
        if (
          expected.id == data.id &&
          expected.title == data.title &&
          expected.blockType == data.blockType &&
          expected.isCompleted == data.isCompleted &&
          expected.markdownText == data.markdownText
        )
          return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/summary/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditSummaryBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const mdedit = await screen.findByPlaceholderText(/текст/i);
    await user.type(mdedit, "asdf");

    const buttonDialog = await screen.findByText(/^загрузить файл$/i);
    await user.click(buttonDialog);

    const fileInput = await screen.findByLabelText(/^выбор файла/i);
    const testFile = new File(["test content"], "test-file.txt", {
      type: "text/plain",
    });
    fireEvent.change(fileInput, { target: { files: [testFile] } });

    const filename = await screen.findByLabelText(/^название/i);
    await user.type(filename, "название1");
    const fileDescription = await screen.findByLabelText(/^описание/i);
    await user.type(fileDescription, "описание2");
    const fileAuthor = await screen.findByLabelText(/^автор/i);
    await user.type(fileAuthor, "Автор3");
    const buttonSaveFile = await screen.findByText(/^загрузить$/i);
    await user.click(buttonSaveFile);

    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeNull();
    });
  });
  it("deletes file", async () => {
    // Arrange
    const initial = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
      fileName: "string",
      fileHref: "string",
    };
    const expected = {
      id: 1,
      title: "title",
      blockType: 1,
      isCompleted: false,
      markdownText: "string",
      fileName: null,
      fileHref: null,
    };
    server.use(
      http.put(backend("/courses/1/edit/summary/1"), async ({ request }) => {
        const data: any = await request.json();
        console.error(data);
        if (
          expected.id == data.id &&
          expected.title == data.title &&
          expected.blockType == data.blockType &&
          expected.isCompleted == data.isCompleted &&
          expected.markdownText == data.markdownText &&
          expected.fileName == data.fileName &&
          expected.fileHref == data.fileHref
        )
          return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/summary/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditSummaryBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act

    const deleteFile = await screen.findByAltText(/^убрать/i);
    await user.click(deleteFile);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeNull();
      expect(
        screen.queryByText(/нет прикрепленного файла/i)
      ).toBeInTheDocument();
    });
  });
  it("handles NaNs as course and block id", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditSummaryBlockComponent courseId={NaN} blockId={NaN} />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
});
