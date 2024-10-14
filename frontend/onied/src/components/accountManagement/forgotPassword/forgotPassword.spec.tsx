import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import ForgotPasswordComponent from "./forgotPassword";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect, test } from "vitest";
import backend from "../../../../test/helpers/backend";

describe("ForgotPasswordComponent", () => {
  it("loads correctly", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/forgotPassword"]}>
        <ForgotPasswordComponent />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/forgotPassword"), () => {
        return new HttpResponse(null, { status: 200 });
      })
    );
    const button = screen.getByText(/отправить/i);

    // Act

    // Assert
    expect(screen.queryByText(/отправлено/i)).toBeNull();
    expect(button).toBeEnabled();
  });
  it("sends email when address correct", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/forgotPassword"]}>
        <ForgotPasswordComponent />
      </MemoryRouter>
    );
    const user = userEvent.setup();
    server.use(
      http.post(backend("/forgotPassword"), () => {
        return new HttpResponse(null, { status: 200 });
      })
    );
    const button = screen.getByText(/отправить/i);

    // Act
    await user.type(screen.getByPlaceholderText(/адрес/i), "asdf@asdf.asdf");
    await user.click(button);

    // Assert
    expect(screen.getByText(/отправлено/i)).toBeInTheDocument();
    expect(button).toBeDisabled();
  });
  test.each([" ", "   ", "@", "asdfs@", "@skldfjskldj", ".@."])(
    "reports when address incorrect",
    async (email: string) => {
      // Arrange
      render(
        <MemoryRouter initialEntries={["/forgotPassword"]}>
          <ForgotPasswordComponent />
        </MemoryRouter>
      );
      const user = userEvent.setup();
      server.use(
        http.post(backend("/forgotPassword"), () => {
          return new HttpResponse(null, { status: 400 });
        })
      );
      const input = screen.getByPlaceholderText(/адрес/i);
      const button = screen.getByText(/отправить/i);
      input.removeAttribute("required");
      input.removeAttribute("type");

      // Act
      await user.type(input, email);
      await user.click(button);

      // Assert
      expect(screen.queryByText(/отправлено/i)).toBeNull();
      expect(button).toBeEnabled();
      expect(screen.getByText(/(неверн|неправиль)/i)).toBeInTheDocument();
    }
  );
  it("shows error when something is wrong on the backend", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/forgotPassword"]}>
        <ForgotPasswordComponent />
      </MemoryRouter>
    );
    const user = userEvent.setup();
    server.use(
      http.post(backend("/forgotPassword"), () => {
        return new HttpResponse(null, { status: 500 });
      })
    );
    const button = screen.getByText(/отправить/i);

    // Act
    await user.type(screen.getByPlaceholderText(/адрес/i), "asdf@asdf.asdf");
    await user.click(button);

    // Assert
    expect(screen.queryByText(/отправлено/i)).toBeNull();
    expect(button).toBeEnabled();
    expect(screen.getByText(/ошибка/i)).toBeInTheDocument();
  });
});
