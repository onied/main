import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import LoginForm from "./loginForm";
import { MemoryRouter, redirect } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../../test/helpers/backend";
import mockedNavigate from "../../../../../test/mocks/useNavigate";

describe("LoginForm", () => {
  it("loads and logs in when 2fa not required", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/login"]}>
        <LoginForm />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/login"), async ({ request }) => {
        const data: any = await request.json();
        if (data.email == "asdf@asdf.asdf" && data.password == "hunter2")
          return HttpResponse.json({
            accessToken: "YWNjZXNzVG9rZW4=",
            expiresIn: 1,
            refreshToken: "cmVmcmVzaFRva2Vu",
          });
        return new HttpResponse(null, { status: 401 });
      })
    );
    const user = userEvent.setup();
    const button = screen.getByText(/^войти$/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/пароль/i);

    // Act
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/");
    });
  });
  it("loads and logs in when 2fa not required and redirects to where it should", async () => {
    // Arrange
    const url = "/otheraddress";
    render(
      <MemoryRouter
        initialEntries={[`/login?redirect=${encodeURIComponent(url)}`]}
      >
        <LoginForm />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/login"), async ({ request }) => {
        const data: any = await request.json();
        if (data.email == "asdf@asdf.asdf" && data.password == "hunter2")
          return HttpResponse.json({
            accessToken: "YWNjZXNzVG9rZW4=",
            expiresIn: 1,
            refreshToken: "cmVmcmVzaFRva2Vu",
          });
        return new HttpResponse(null, { status: 401 });
      })
    );
    const user = userEvent.setup();
    const button = screen.getByText(/^войти$/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/пароль/i);

    // Act
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(url);
    });
  });
  it("loads and logs in when 2fa not required and does not redirect to other origins", async () => {
    // Arrange
    const url = "http://malicious.url";
    render(
      <MemoryRouter
        initialEntries={[`/login?redirect=${encodeURIComponent(url)}`]}
      >
        <LoginForm />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/login"), async ({ request }) => {
        const data: any = await request.json();
        if (data.email == "asdf@asdf.asdf" && data.password == "hunter2")
          return HttpResponse.json({
            accessToken: "YWNjZXNzVG9rZW4=",
            expiresIn: 1,
            refreshToken: "cmVmcmVzaFRva2Vu",
          });
        return new HttpResponse(null, { status: 401 });
      })
    );
    const user = userEvent.setup();
    const button = screen.getByText(/^войти$/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/пароль/i);

    // Act
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/");
    });
  });
  it("reports an error when 2fa not required and login unsuccessful", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/login"]}>
        <LoginForm />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/login"), async () => {
        return new HttpResponse(null, { status: 401 });
      })
    );
    const user = userEvent.setup();
    const button = screen.getByText(/^войти$/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/пароль/i);

    // Act
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/неверн/i)).toBeInTheDocument();
    });
  });
  it("redirects to 2fa page when 2fa required", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/login"]}>
        <LoginForm />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/login"), async () => {
        return HttpResponse.json(
          { detail: "RequiresTwoFactor" },
          { status: 401 }
        );
      })
    );
    const user = userEvent.setup();
    const button = screen.getByText(/^войти$/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/пароль/i);

    // Act
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(
        "/login/2fa",
        expect.objectContaining({
          state: expect.objectContaining({
            email: "asdf@asdf.asdf",
            password: "hunter2",
          }),
        })
      );
    });
  });
  it("redirects to 2fa page when 2fa required and passes redirect", async () => {
    // Arrange
    const url = "/otheraddress";
    render(
      <MemoryRouter
        initialEntries={[`/login?redirect=${encodeURIComponent(url)}`]}
      >
        <LoginForm />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/login"), async () => {
        return HttpResponse.json(
          { detail: "RequiresTwoFactor" },
          { status: 401 }
        );
      })
    );
    const user = userEvent.setup();
    const button = screen.getByText(/^войти$/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/пароль/i);

    // Act
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(
        "/login/2fa",
        expect.objectContaining({
          state: expect.objectContaining({
            email: "asdf@asdf.asdf",
            password: "hunter2",
            redirect: url,
          }),
        })
      );
    });
  });
  it("logs in with 2fa code if it's provided", async () => {
    // Arrange
    server.use(
      http.post(backend("/login"), async ({ request }) => {
        console.error("sasdf");
        const data: any = await request.json();
        if (
          data.email === "asdf@asdf.asdf" &&
          data.password === "hunter2" &&
          data.twoFactorCode === "123456"
        )
          return HttpResponse.json();
        return HttpResponse.json({ detail: "Failed" }, { status: 401 });
      })
    );
    render(
      <MemoryRouter
        initialEntries={[
          {
            pathname: "/login",
            state: {
              email: "asdf@asdf.asdf",
              password: "hunter2",
              twoFactorCode: "123456",
            },
          },
        ]}
      >
        <LoginForm />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/");
    });
  });
  it("logs in with 2fa and redirects to where it should", async () => {
    // Arrange
    const url = "/otheraddress";
    server.use(
      http.post(backend("/login"), async ({ request }) => {
        console.error("sasdf");
        const data: any = await request.json();
        if (
          data.email === "asdf@asdf.asdf" &&
          data.password === "hunter2" &&
          data.twoFactorCode === "123456"
        )
          return HttpResponse.json();
        return HttpResponse.json({ detail: "Failed" }, { status: 401 });
      })
    );
    render(
      <MemoryRouter
        initialEntries={[
          {
            pathname: "/login",
            state: {
              email: "asdf@asdf.asdf",
              password: "hunter2",
              twoFactorCode: "123456",
              redirect: encodeURIComponent(url),
            },
          },
        ]}
      >
        <LoginForm />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(url);
    });
  });
  it("logs in with 2fa and does not redirect to other origins", async () => {
    // Arrange
    const url = "http://malicious.url";
    server.use(
      http.post(backend("/login"), async ({ request }) => {
        console.error("sasdf");
        const data: any = await request.json();
        if (
          data.email === "asdf@asdf.asdf" &&
          data.password === "hunter2" &&
          data.twoFactorCode === "123456"
        )
          return HttpResponse.json();
        return HttpResponse.json({ detail: "Failed" }, { status: 401 });
      })
    );
    render(
      <MemoryRouter
        initialEntries={[
          {
            pathname: "/login",
            state: {
              email: "asdf@asdf.asdf",
              password: "hunter2",
              twoFactorCode: "123456",
              redirect: encodeURIComponent(url),
            },
          },
        ]}
      >
        <LoginForm />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/");
    });
  });
  test.each([
    [500, {}, /ошибка/i],
    [400, { detail: "LockedOut" }, /(подождите|позже)/i],
    [401, {}, /данные/i],
  ])("reports errors from backend", async (status, response, expected) => {
    // Arrange
    server.use(
      http.post(backend("/login"), async () => {
        return HttpResponse.json(response, { status: status });
      })
    );
    render(
      <MemoryRouter
        initialEntries={[
          {
            pathname: "/login",
            state: {
              email: "asdf@asdf.asdf",
              password: "hunter2",
              twoFactorCode: "123456",
            },
          },
        ]}
      >
        <LoginForm />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(expected)).toBeInTheDocument();
    });
  });
});
