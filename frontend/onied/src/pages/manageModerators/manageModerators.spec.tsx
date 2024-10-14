import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import ManageModerators from "./manageModerators";
import { http, HttpResponse } from "msw";
import { server } from "../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../test/helpers/backend";
import { MemoryRouter, Route, Routes } from "react-router-dom";

describe("ManageModerators", () => {
  it("renders correctly", async () => {
    // Arrange
    const initial = {
      courseId: 1,
      title: "Course title",
      students: [
        {
          studentId: "60dd5e2a-5fbf-4fd3-80c6-53e3a3fd6118",
          firstName: "First Name",
          lastName: "Last Name",
          avatarHref: null,
          isModerator: false,
        },
      ],
    };
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/назначить/i)).toBeInTheDocument();
      expect(screen.queryByText(/удалить/i)).not.toBeInTheDocument();
    });
  });
  it("appoints moderators", async () => {
    // Arrange
    const initial = {
      courseId: 1,
      title: "Course title",
      students: [
        {
          studentId: "60dd5e2a-5fbf-4fd3-80c6-53e3a3fd6118",
          firstName: "First Name",
          lastName: "Last Name",
          avatarHref: null,
          isModerator: false,
        },
      ],
    };
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json(initial);
      }),
      http.patch(backend("/courses/1/moderators/add"), async ({ request }) => {
        const data: any = await request.json();
        if (data.studentId == initial.students[0].studentId)
          return HttpResponse.json();
        return HttpResponse.json({}, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/назначить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/назначить/i)).not.toBeInTheDocument();
      expect(screen.queryByText(/удалить/i)).toBeInTheDocument();
    });
  });
  it("deletes moderators", async () => {
    // Arrange
    const initial = {
      courseId: 1,
      title: "Course title",
      students: [
        {
          studentId: "60dd5e2a-5fbf-4fd3-80c6-53e3a3fd6118",
          firstName: "First Name",
          lastName: "Last Name",
          avatarHref: null,
          isModerator: true,
        },
      ],
    };
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json(initial);
      }),
      http.patch(
        backend("/courses/1/moderators/delete"),
        async ({ request }) => {
          const data: any = await request.json();
          if (data.studentId == initial.students[0].studentId)
            return HttpResponse.json();
          return HttpResponse.json({}, { status: 404 });
        }
      )
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/удалить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/назначить/i)).toBeInTheDocument();
      expect(screen.queryByText(/удалить/i)).not.toBeInTheDocument();
    });
  });
  it("handles invalid courseId", async () => {
    // Arrange
    const initial = {
      courseId: 1,
      title: "Course title",
      students: [
        {
          studentId: "60dd5e2a-5fbf-4fd3-80c6-53e3a3fd6118",
          firstName: "First Name",
          lastName: "Last Name",
          avatarHref: null,
          isModerator: false,
        },
      ],
    };
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json(initial);
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/asdf" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
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
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json({}, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports not found while adding moderators", async () => {
    // Arrange
    const initial = {
      courseId: 1,
      title: "Course title",
      students: [
        {
          studentId: "60dd5e2a-5fbf-4fd3-80c6-53e3a3fd6118",
          firstName: "First Name",
          lastName: "Last Name",
          avatarHref: null,
          isModerator: false,
        },
      ],
    };
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json(initial);
      }),
      http.patch(backend("/courses/1/moderators/add"), () => {
        return HttpResponse.json({}, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/назначить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports not found while deleting moderators", async () => {
    // Arrange
    const initial = {
      courseId: 1,
      title: "Course title",
      students: [
        {
          studentId: "60dd5e2a-5fbf-4fd3-80c6-53e3a3fd6118",
          firstName: "First Name",
          lastName: "Last Name",
          avatarHref: null,
          isModerator: true,
        },
      ],
    };
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json(initial);
      }),
      http.patch(backend("/courses/1/moderators/delete"), () => {
        return HttpResponse.json({}, { status: 404 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/удалить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("reports forbidden", async () => {
    // Arrange
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json({}, { status: 403 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нет прав/i)).toBeInTheDocument();
    });
  });
  it("reports forbidden while adding moderators", async () => {
    // Arrange
    const initial = {
      courseId: 1,
      title: "Course title",
      students: [
        {
          studentId: "60dd5e2a-5fbf-4fd3-80c6-53e3a3fd6118",
          firstName: "First Name",
          lastName: "Last Name",
          avatarHref: null,
          isModerator: false,
        },
      ],
    };
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json(initial);
      }),
      http.patch(backend("/courses/1/moderators/add"), () => {
        return HttpResponse.json({}, { status: 403 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/назначить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нет прав/i)).toBeInTheDocument();
    });
  });
  it("reports forbidden while deleting moderators", async () => {
    // Arrange
    const initial = {
      courseId: 1,
      title: "Course title",
      students: [
        {
          studentId: "60dd5e2a-5fbf-4fd3-80c6-53e3a3fd6118",
          firstName: "First Name",
          lastName: "Last Name",
          avatarHref: "https://example.com",
          isModerator: true,
        },
      ],
    };
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json(initial);
      }),
      http.patch(backend("/courses/1/moderators/delete"), () => {
        return HttpResponse.json({}, { status: 403 });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/удалить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нет прав/i)).toBeInTheDocument();
    });
  });
  it("reports no students", async () => {
    // Arrange
    server.use(
      http.get(backend("/courses/1/moderators"), () => {
        return HttpResponse.json({ students: [] });
      })
    );
    render(
      <MemoryRouter initialEntries={[{ pathname: "/moderators/1" }]}>
        <Routes>
          <Route path="/moderators/:courseId" element={<ManageModerators />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/нельзя назначить/i)).toBeInTheDocument();
    });
  });
});
