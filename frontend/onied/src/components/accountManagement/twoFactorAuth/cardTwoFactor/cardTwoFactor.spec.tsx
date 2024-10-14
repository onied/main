import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import CardTwoFactor from "./cardTwoFactor";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../../test/helpers/backend";
import mockedNavigate from "../../../../../test/mocks/useNavigate";

describe("CardTwoFactor", () => {
  it("redirects to login when state not provided", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={[{ pathname: "/login/2fa" }]}>
        <CardTwoFactor />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/login");
    });
  });

  it("logs in", async () => {
    // Arrange
    render(
      <MemoryRouter
        initialEntries={[
          {
            pathname: "/login/2fa",
            state: { email: "asdf@asdf.asdf", password: "hunter2" },
          },
        ]}
      >
        <CardTwoFactor />
      </MemoryRouter>
    );

    server.use(
      http.post(backend("/login"), async ({ request }) => {
        const data: any = await request.json();
        if (
          data.email == "asdf@asdf.asdf" &&
          data.password == "hunter2" &&
          data.twoFactorCode == 123456
        )
          return HttpResponse.json({
            accessToken: "YWNjZXNzVG9rZW4=",
            expiresIn: 1,
            refreshToken: "cmVmcmVzaFRva2Vu",
          });
        return new HttpResponse(null, { status: 401 });
      })
    );
    const button = screen.getByText(/подтвердить/i);
    const user = userEvent.setup();

    // Act
    await user.keyboard("{tab}123456");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/");
    });
  });

  it("reports when there's no code", async () => {
    // Arrange
    render(
      <MemoryRouter
        initialEntries={[
          {
            pathname: "/login/2fa",
            state: { email: "asdf@asdf.asdf", password: "hunter2" },
          },
        ]}
      >
        <CardTwoFactor />
      </MemoryRouter>
    );

    server.use(
      http.post(backend("/login"), async ({ request }) => {
        const data: any = await request.json();
        if (
          data.email == "asdf@asdf.asdf" &&
          data.password == "hunter2" &&
          data.twoFactorCode == 123456
        )
          return HttpResponse.json({
            accessToken: "YWNjZXNzVG9rZW4=",
            expiresIn: 1,
            refreshToken: "cmVmcmVzaFRva2Vu",
          });
        return new HttpResponse(null, { status: 401 });
      })
    );
    const button = screen.getByText(/подтвердить/i);
    const user = userEvent.setup();

    // Act
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/не заполнен/i)).toBeInTheDocument();
    });
  });

  test.each([
    [500, {}, /ошибка/i],
    [400, { detail: "LockedOut" }, /подождите/i],
    [401, {}, /данные/i],
  ])("reports errors from backend", async (status, response, expected) => {
    // Arrange
    render(
      <MemoryRouter
        initialEntries={[
          {
            pathname: "/login/2fa",
            state: { email: "asdf@asdf.asdf", password: "hunter2" },
          },
        ]}
      >
        <CardTwoFactor />
      </MemoryRouter>
    );

    server.use(
      http.post(backend("/login"), async () => {
        return HttpResponse.json(response, { status: status });
      })
    );
    const button = screen.getByText(/подтвердить/i);
    const user = userEvent.setup();

    // Act
    await user.keyboard("{tab}123456");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(
        "/login",
        expect.objectContaining({
          state: {
            errorMessage: expect.stringMatching(expected),
          },
        })
      );
    });
  });
});
