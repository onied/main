import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import EditVideoBlockComponent from "./editVideoBlock";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "@onied/tests/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "@onied/tests//helpers/backend";

describe.skip("EditVideoBlockComponent", () => {
  it("renders correctly", async () => {
    // Arrange
    const initial = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    const expected = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    server.use(
      http.put(backend("/courses/1/edit/video/1"), async ({ request }) => {
        const data: any = await request.json();
        if (data.href == expected) return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/video/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/video"]}>
        <EditVideoBlockComponent courseId={1} blockId={1} />
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
    const initial = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    const expected = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    server.use(
      http.put(backend("/courses/1/edit/video/1"), async ({ request }) => {
        const data: any = await request.json();
        if (data.href == expected) return HttpResponse.json();
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1/video/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/video"]}>
        <EditVideoBlockComponent courseId={1} blockId={1} />
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
    const expected = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    server.use(
      http.put(backend("/courses/1/edit/video/1"), async ({ request }) => {
        const data: any = await request.json();
        if (data.href == expected) return HttpResponse.json();
        //         return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/video/1"), () => {
        return HttpResponse.json({}, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/video"]}>
        <EditVideoBlockComponent courseId={1} blockId={1} />
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
    const initial = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    server.use(
      http.put(backend("/courses/1/edit/video/1"), () => {
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/video/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/video"]}>
        <EditVideoBlockComponent courseId={1} blockId={1} />
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
  it("reports when not found while saving", async () => {
    // Arrange
    const initial = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    server.use(
      http.put(backend("/courses/1/edit/video/1"), () => {
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/video/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/video"]}>
        <EditVideoBlockComponent courseId={1} blockId={1} />
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
  test.each([
    "https://vk.com/video?q=never%20gonna%20give%20you%20up&z=video50861944_456251300%2Fpl_cat_trends",
    "https://vk.com/video/@vkvideo?z=video-220754053_456240120%2Fclub220754053%2Fpl_-220754053_-2",
    "https://rutube.ru/video/c6cc4d620b1d4338901770a44b3e82f4/",
    "https://www.youtube.com/watch?v=cPCLFtxpadE",
  ])("actually saves", async (url) => {
    // Arrange
    const initial = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
    const expected = url;
    server.use(
      http.put(backend("/courses/1/edit/video/1"), async ({ request }) => {
        const data: any = await request.json();
        if (data.href == expected) return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/courses/1/edit/check-edit-course"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1/video/1"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/video"]}>
        <EditVideoBlockComponent courseId={1} blockId={1} />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const field = await screen.findByPlaceholderText(
      /вставьте сюда ссылку на видео/i
    );
    await user.type(field, expected);
    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeNull();
    });
  });
  it("handles NaNs as course and block id", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/edit/summary"]}>
        <EditVideoBlockComponent courseId={NaN} blockId={NaN} />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
});
