import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import LoginForm from "./loginForm";
import { MemoryRouter } from "react-router-dom";
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
      http.get(backend("/manage/2fa/info"), ({ request }) => {
        const data = new URL(request.url).searchParams;
        if (data.get("email") == "asdf@asdf.asdf")
          return HttpResponse.json({
            isTwoFactorEnabled: false,
          });
        return new HttpResponse(null, { status: 404 });
      }),
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
      http.get(backend("/manage/2fa/info"), ({ request }) => {
        const data = new URL(request.url).searchParams;
        if (data.get("email") == "asdf@asdf.asdf")
          return HttpResponse.json({
            isTwoFactorEnabled: false,
          });
        return new HttpResponse(null, { status: 404 });
      }),
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
      http.get(backend("/manage/2fa/info"), ({ request }) => {
        const data = new URL(request.url).searchParams;
        if (data.get("email") == "asdf@asdf.asdf")
          return HttpResponse.json({
            isTwoFactorEnabled: false,
          });
        return new HttpResponse(null, { status: 404 });
      }),
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
      http.get(backend("/manage/2fa/info"), ({ request }) => {
        const data = new URL(request.url).searchParams;
        if (data.get("email") == "asdf@asdf.asdf")
          return HttpResponse.json({
            isTwoFactorEnabled: false,
          });
        return new HttpResponse(null, { status: 404 });
      }),
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
  it("reports an error when 2fa request not successful", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/login"]}>
        <LoginForm />
      </MemoryRouter>
    );
    server.use(
      http.get(backend("/manage/2fa/info"), () => {
        return new HttpResponse(null, { status: 404 });
      }),
      http.post(backend("/login"), async () => {
        return new HttpResponse(null, { status: 200 });
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
      http.get(backend("/manage/2fa/info"), ({ request }) => {
        const data = new URL(request.url).searchParams;
        if (data.get("email") == "asdf@asdf.asdf")
          return HttpResponse.json({
            isTwoFactorEnabled: true,
          });
        return new HttpResponse(null, { status: 404 });
      }),
      http.post(backend("/login"), async () => {
        return new HttpResponse(null, { status: 200 });
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
        expect.anything()
      );
    });
  });
  it("displays an error if it's provided", async () => {
    // Arrange
    render(
      <MemoryRouter
        initialEntries={[
          { pathname: "/login", state: { errorMessage: "error" } },
        ]}
      >
        <LoginForm />
      </MemoryRouter>
    );

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.getByText("error")).toBeInTheDocument();
    });
  });
});
