import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import EditPreviewComponent from "./editPreview";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../test/helpers/backend";
import imagePlaceholder from "../../../assets/imagePlaceholder.svg";

describe("EditPreviewComponent", () => {
  it("renders correctly", async () => {
    // Arrange
    const initial = {
      title: "string",
      pictureHref: "",
      description: "string",
      hoursCount: 1,
      price: 0,
      category: {
        id: 1,
        name: "sdjlkf",
      },
      courseAuthor: {
        name: "string",
        avatarHref: "http://example.com",
      },
      isArchived: false,
      hasCertificates: false,
      isProgramVisible: true,
      courseProgram: ["asdf", "jsldkjflkj", "i am really tired"],
    };
    const expected = {
      title: "string",
      description: "string",
      categoryId: 1,
      price: 0,
      hoursCount: 1,
      pictureHref: imagePlaceholder,
      isProgramVisible: true,
      hasCertificates: false,
      isArchived: false,
    };
    server.use(
      http.put(backend("/courses/1/edit"), async ({ request }) => {
        const data: any = await request.json();
        if (
          data.title == expected.title &&
          data.description == expected.description &&
          data.categoryId == expected.categoryId &&
          data.hoursCount == expected.hoursCount &&
          data.pictureHref == expected.pictureHref &&
          data.isProgramVisible == expected.isProgramVisible &&
          data.hasCertificates == expected.hasCertificates &&
          data.isArchived == expected.isArchived
        )
          return HttpResponse.json(initial);
        return HttpResponse.json({}, { status: 400 });
      }),
      http.get(backend("/courses/1"), () => {
        return HttpResponse.json(initial);
      }),
      http.get(backend("/categories"), () => {
        return HttpResponse.json([
          {
            id: 1,
            name: "sdjlkf",
          },
        ]);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/1"]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditPreviewComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/сохранено/i)).toBeInTheDocument();
    });
  });
  it("reports when no access", async () => {
    // Arrange
    const initial = {
      title: "string",
      pictureHref: "",
      description: "string",
      hoursCount: 1,
      price: 0,
      category: {
        id: 1,
        name: "sdjlkf",
      },
      courseAuthor: {
        name: "string",
        avatarHref: "http://example.com",
      },
      isArchived: false,
      hasCertificates: false,
      isProgramVisible: true,
      courseProgram: ["asdf", "jsldkjflkj", "i am really tired"],
    };
    server.use(
      http.put(backend("/courses/1/edit"), () => {
        return HttpResponse.json({}, { status: 403 });
      }),
      http.get(backend("/courses/1"), () => {
        return HttpResponse.json(initial);
      }),
      http.get(backend("/categories"), () => {
        return HttpResponse.json([
          {
            id: 1,
            name: "sdjlkf",
          },
        ]);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/1"]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditPreviewComponent />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нет доступа/i)).toBeInTheDocument();
    });
  });
  it("reports when not found", async () => {
    // Arrange
    const expected = {
      title: "string",
      description: "string",
      categoryId: 1,
      price: 0,
      hoursCount: 1,
      pictureHref: imagePlaceholder,
      isProgramVisible: true,
      hasCertificates: false,
      isArchived: false,
    };
    server.use(
      http.put(backend("/courses/1/edit"), async ({ request }) => {
        const data: any = await request.json();
        if (
          data.title == expected.title &&
          data.description == expected.description &&
          data.categoryId == expected.categoryId &&
          data.hoursCount == expected.hoursCount &&
          data.pictureHref == expected.pictureHref &&
          data.isProgramVisible == expected.isProgramVisible &&
          data.hasCertificates == expected.hasCertificates &&
          data.isArchived == expected.isArchived
        )
          return HttpResponse.json(expected);
        return HttpResponse.json({}, { status: 400 });
      }),
      http.get(backend("/courses/1"), () => {
        return HttpResponse.json({}, { status: 404 });
      }),
      http.get(backend("/categories"), () => {
        return HttpResponse.json([
          {
            id: 1,
            name: "sdjlkf",
          },
        ]);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/1"]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditPreviewComponent />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("actually saves", async () => {
    // Arrange
    const initial = {
      title: "string",
      pictureHref: "",
      description: "string",
      hoursCount: 1,
      price: 0,
      category: {
        id: 1,
        name: "sdjlkf",
      },
      courseAuthor: {
        name: "string",
        avatarHref: "http://example.com",
      },
      isArchived: false,
      hasCertificates: false,
      isProgramVisible: true,
      courseProgram: ["asdf", "jsldkjflkj", "i am really tired"],
    };
    const expected = {
      title: "stringasdf",
      pictureHref: backend(imagePlaceholder),
      description: "stringasdfasdfasdf",
      hoursCount: 11,
      price: 1,
      category: {
        id: 1,
        name: "sdjlkf",
      },
      courseAuthor: {
        name: "string",
        avatarHref: "http://example.com",
      },
      isArchived: true,
      hasCertificates: true,
      isProgramVisible: false,
      courseProgram: [],
    };
    server.use(
      http.put(backend("/courses/1/edit"), async ({ request }) => {
        const data: any = await request.json();
        if (
          data.title == expected.title &&
          data.description == expected.description &&
          data.categoryId == expected.category.id &&
          data.hoursCount == expected.hoursCount &&
          data.pictureHref == expected.pictureHref &&
          data.isProgramVisible == expected.isProgramVisible &&
          data.hasCertificates == expected.hasCertificates &&
          data.isArchived == expected.isArchived
        )
          return HttpResponse.json(expected);
        return HttpResponse.json({}, { status: 400 });
      }),
      http.get(backend("/courses/1"), () => {
        return HttpResponse.json(initial);
      }),
      http.get(backend("/categories"), () => {
        return HttpResponse.json([
          {
            id: 1,
            name: "sdjlkf",
          },
        ]);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/1"]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditPreviewComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const title = await screen.findByLabelText(/^название$/i);
    await user.type(title, "asdf");
    const description = await screen.findByLabelText(/^описание$/i);
    await user.type(description, "asdfasdfasdf");
    const price = await screen.findByLabelText(/^цена/i);
    await user.type(price, "1");
    const hoursCount = await screen.findByLabelText(/^время/i);
    await user.type(hoursCount, "1");
    const programVisible = await screen.findByLabelText(/^содержание курса$/i);
    await user.click(programVisible);
    const certificates = await screen.findByLabelText(/^выдача сертификатов$/i);
    await user.click(certificates);
    const archive = await screen.findByLabelText(/в архив/i);
    await user.click(archive);

    const uploadButton = await screen.findByText(/^загрузить$/i);
    await user.click(uploadButton);
    const pictureHref = await screen.findByPlaceholderText(/ссылка/i);
    await user.type(pictureHref, backend(imagePlaceholder));
    const dialogButton = await screen.findByText(/^сохранить$/i);
    await user.click(dialogButton);

    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/сохранено/i)).toBeInTheDocument();
    });
  });
  it("displays errors when fields empty", async () => {
    // Arrange
    const initial = {
      title: "",
      pictureHref: "",
      description: "",
      hoursCount: -1,
      price: -1,
      category: {
        id: 1,
        name: "sdjlkf",
      },
      courseAuthor: {
        name: "string",
        avatarHref: "http://example.com",
      },
      isArchived: false,
      hasCertificates: false,
      isProgramVisible: true,
      courseProgram: [],
    };
    server.use(
      http.put(backend("/courses/1/edit"), () => {
        return HttpResponse.json(
          {
            errors: {
              Title: true,
              Description: true,
              PictureHref: true,
              HoursCount: true,
              Price: true,
            },
          },
          { status: 400 }
        );
      }),
      http.get(backend("/courses/1"), () => {
        return HttpResponse.json(initial);
      }),
      http.get(backend("/categories"), () => {
        return HttpResponse.json([
          {
            id: 1,
            name: "sdjlkf",
          },
        ]);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/1"]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditPreviewComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/сохранено/i)).toBeNull();
      expect(screen.queryAllByText(/это обязательное поле/i)).toHaveLength(2);
      expect(screen.queryByText(/введите корректный URL/i)).toBeInTheDocument();
      expect(screen.queryAllByText(/введите число/i)).toHaveLength(2);
    });
  });
  it("displays errors when fields are not valid", async () => {
    // Arrange
    const initial = {
      title: "string",
      pictureHref: "asdf",
      description: "asdfasdfasdfasdf",
      hoursCount: -1,
      price: -1,
      category: {
        id: 1,
        name: "sdjlkf",
      },
      courseAuthor: {
        name: "string",
        avatarHref: "http://example.com",
      },
      isArchived: false,
      hasCertificates: false,
      isProgramVisible: true,
      courseProgram: [],
    };
    server.use(
      http.put(backend("/courses/1/edit"), () => {
        return HttpResponse.json(
          {
            errors: {
              Title: true,
              Description: true,
              PictureHref: true,
              HoursCount: true,
              Price: true,
            },
          },
          { status: 400 }
        );
      }),
      http.get(backend("/courses/1"), () => {
        return HttpResponse.json(initial);
      }),
      http.get(backend("/categories"), () => {
        return HttpResponse.json([
          {
            id: 1,
            name: "sdjlkf",
          },
        ]);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/1"]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditPreviewComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^сохранить изменения$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/сохранено/i)).toBeNull();
      expect(screen.queryByText(/название не может/i)).toBeInTheDocument();
      expect(screen.queryByText(/описание не может/i)).toBeInTheDocument();
      expect(screen.queryByText(/введите корректный URL/i)).toBeInTheDocument();
      expect(screen.queryAllByText(/введите число/i)).toHaveLength(2);
    });
  });
  it("displays preview modal", async () => {
    // Arrange
    const initial = {
      title: "string",
      pictureHref: "asdf",
      description: "asdfasdfasdfasdf",
      hoursCount: -1,
      price: -1,
      category: {
        id: 1,
        name: "sdjlkf",
      },
      courseAuthor: {
        name: "courseAuthor",
        avatarHref: "http://example.com",
      },
      isArchived: false,
      hasCertificates: false,
      isProgramVisible: true,
      courseProgram: [],
    };
    server.use(
      http.put(backend("/courses/1/edit"), () => {
        return HttpResponse.json();
      }),
      http.get(backend("/courses/1"), () => {
        return HttpResponse.json(initial);
      }),
      http.get(backend("/categories"), () => {
        return HttpResponse.json([
          {
            id: 1,
            name: "sdjlkf",
          },
        ]);
      })
    );
    render(
      <MemoryRouter initialEntries={["/edit/1"]}>
        <Routes>
          <Route path="/edit/:courseId" element={<EditPreviewComponent />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^посмотреть$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(initial.courseAuthor.name)).toBeInTheDocument();
    });
  });
});
