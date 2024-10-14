import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import CoursePurchase from "./index";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect, vi } from "vitest";
import backend from "../../../../test/helpers/backend";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import mockedNavigate from "../../../../test/mocks/useNavigate";

describe("CoursePurchase", () => {
  it("renders correctly", async () => {
    // Arrange
    server.use(
      http.get(backend("/purchases/new/course"), ({ request }) => {
        const params = new URL(request.url).searchParams;
        if (params.get("courseId") == "1")
          return HttpResponse.json({
            id: 1,
            title: "Course",
            price: 100,
          });
        return HttpResponse.json({}, { status: 400 });
      }),
      http.post(backend("/purchases/new/course"), async ({ request }) => {
        const data: any = await request.json();
        if (data.courseId == 1) return HttpResponse.json();
        return HttpResponse.json({}, { status: 400 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/purchase/1" }]}>
        <Routes>
          <Route path="/purchase/:courseId" element={<CoursePurchase />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/course/i)).toBeInTheDocument();
      expect(screen.queryByText(/100/i)).toBeInTheDocument();
    });
  });
  it("reports not found", async () => {
    // Arrange
    server.use(
      http.get(backend("/purchases/new/course"), ({ request }) => {
        const params = new URL(request.url).searchParams;
        if (params.get("courseId") == "21")
          return HttpResponse.json({
            id: 1,
            title: "Course",
            price: 100,
          });
        return HttpResponse.json({}, { status: 404 });
      }),
      http.post(backend("/purchases/new/course"), async ({ request }) => {
        const data: any = await request.json();
        if (data.courseId == 1) return HttpResponse.json();
        return HttpResponse.json({}, { status: 400 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter initialEntries={[{ pathname: "/purchase/1" }]}>
        <Routes>
          <Route path="/purchase/:courseId" element={<CoursePurchase />} />
        </Routes>
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/не найден/i)).toBeInTheDocument();
    });
  });
  it("orders correctly", async () => {
    // Arrange
    server.use(
      http.get(backend("/purchases/new/course"), ({ request }) => {
        const params = new URL(request.url).searchParams;
        if (params.get("courseId") == "1")
          return HttpResponse.json({
            id: 1,
            title: "Course",
            price: 100,
          });
        return HttpResponse.json({}, { status: 400 });
      }),
      http.post(backend("/purchases/new/course"), async ({ request }) => {
        const data: any = await request.json();
        if (data.courseId == 1) return HttpResponse.json();
        return HttpResponse.json({}, { status: 400 });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter
        initialEntries={[
          { pathname: "/purchase/1", state: { address: "asdfasdfasdf" } },
        ]}
      >
        <Routes>
          <Route path="/purchase/:courseId" element={<CoursePurchase />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const cardNumber = await screen.findByPlaceholderText(/номер карты/i);
    await user.type(cardNumber, "1111111111111111");
    const cardHolder = await screen.findByPlaceholderText(/держатель карты/i);
    await user.type(cardHolder, "Ivan Ivanov");
    const month = await screen.findByPlaceholderText(/мм/i);
    await user.type(month, "09");
    const year = await screen.findByPlaceholderText(/гг/i);
    await user.type(year, "09");
    const cvc = await screen.findByPlaceholderText(/cvc/i);
    await user.type(cvc, "123");

    const button = await screen.findByText(/оплатить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(-1);
    });
  });
  test.each([
    [400, /ошибка.* валидации/i],
    [403, /не можете/i],
    [500, /ошибка/i],
  ])("displays errors", async (status, expected) => {
    // Arrange
    server.use(
      http.get(backend("/purchases/new/course"), ({ request }) => {
        const params = new URL(request.url).searchParams;
        if (params.get("courseId") == "1")
          return HttpResponse.json({
            id: 1,
            title: "Course",
            price: 100,
          });
        return HttpResponse.json({}, { status: 400 });
      }),
      http.post(backend("/purchases/new/course"), async () => {
        return HttpResponse.json({}, { status: status });
      })
    );
    global.alert = vi.fn();
    render(
      <MemoryRouter
        initialEntries={[
          { pathname: "/purchase/1", state: { address: "asdfasdfasdf" } },
        ]}
      >
        <Routes>
          <Route path="/purchase/:courseId" element={<CoursePurchase />} />
        </Routes>
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const cardNumber = await screen.findByPlaceholderText(/номер карты/i);
    await user.type(cardNumber, "1111111111111111");
    const cardHolder = await screen.findByPlaceholderText(/держатель карты/i);
    await user.type(cardHolder, "Ivan Ivanov");
    const month = await screen.findByPlaceholderText(/мм/i);
    await user.type(month, "09");
    const year = await screen.findByPlaceholderText(/гг/i);
    await user.type(year, "09");
    const cvc = await screen.findByPlaceholderText(/cvc/i);
    await user.type(cvc, "123");

    const button = await screen.findByText(/оплатить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(expected)).toBeInTheDocument();
    });
  });
});
