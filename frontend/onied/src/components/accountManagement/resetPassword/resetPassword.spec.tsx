import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import ResetPasswordComponent from "./resetPassword";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../test/helpers/backend";
import mockedNavigate from "../../../../test/mocks/useNavigate";

describe("ResetPasswordComponent", () => {
  it("resets password", async () => {
    // Arrange
    const email = "asdf@asdf.asdf";
    const code = "haskjdfhkasdhf";
    const url = `/resetPassword?email=${email}&code=${code}`;
    render(
      <MemoryRouter initialEntries={[url]}>
        <ResetPasswordComponent />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/resetPassword"), () => {
        return new HttpResponse(null, { status: 200 });
      })
    );
    const button = screen.getByText(/сменить/i);
    const password = screen.getByPlaceholderText(/^пароль/i);
    const repeatPassword = screen.getByPlaceholderText(/повтор/i);
    const user = userEvent.setup();

    // Act
    await user.type(password, "Hunter2");
    await user.type(repeatPassword, "Hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/login");
    });
  });
  it("reports when password and repeated password differ", async () => {
    // Arrange
    const email = "asdf@asdf.asdf";
    const code = "haskjdfhkasdhf";
    const url = `/resetPassword?email=${email}&code=${code}`;
    render(
      <MemoryRouter initialEntries={[url]}>
        <ResetPasswordComponent />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/resetPassword"), () => {
        return new HttpResponse(null, { status: 200 });
      })
    );
    const button = screen.getByText(/сменить/i);
    const password = screen.getByPlaceholderText(/^пароль/i);
    const repeatPassword = screen.getByPlaceholderText(/повтор/i);
    const user = userEvent.setup();

    // Act
    await user.type(password, "Hunter2");
    await user.type(repeatPassword, "Hunter");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/совпадают/i)).toBeInTheDocument();
    });
  });
  test.each([
    [{ InvalidToken: true }, /срок/i],
    [{ PasswordRequiresDigit: true }, /цифр/i],
    [{ PasswordRequiresNonAlphanumeric: true }, /символ/i],
    [{ PasswordRequiresUpper: true }, /заглавн/i],
    [{ PasswordRequiresLower: true }, /прописн/i],
    [{ PasswordTooShort: true }, /короткий/i],
    [{ PasswordTooEasy: true }, /простой/i],
  ])("reports when password too easy", async (error: any, expected) => {
    // Arrange
    const email = "asdf@asdf.asdf";
    const code = "haskjdfhkasdhf";
    const url = `/resetPassword?email=${email}&code=${code}`;
    render(
      <MemoryRouter initialEntries={[url]}>
        <ResetPasswordComponent />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/resetPassword"), () => {
        return HttpResponse.json({ errors: error }, { status: 400 });
      })
    );
    const button = screen.getByText(/сменить/i);
    const password = screen.getByPlaceholderText(/^пароль/i);
    const repeatPassword = screen.getByPlaceholderText(/повтор/i);
    const user = userEvent.setup();

    // Act
    await user.type(password, "Hunter2");
    await user.type(repeatPassword, "Hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(expected)).toBeInTheDocument();
    });
  });
  it("reports when server gives invalid response", async () => {
    // Arrange
    const email = "asdf@asdf.asdf";
    const code = "haskjdfhkasdhf";
    const url = `/resetPassword?email=${email}&code=${code}`;
    render(
      <MemoryRouter initialEntries={[url]}>
        <ResetPasswordComponent />
      </MemoryRouter>
    );
    server.use(
      http.post(backend("/resetPassword"), () => {
        return new HttpResponse(null, { status: 500 });
      })
    );
    const button = screen.getByText(/сменить/i);
    const password = screen.getByPlaceholderText(/^пароль/i);
    const repeatPassword = screen.getByPlaceholderText(/повтор/i);
    const user = userEvent.setup();

    // Act
    await user.type(password, "Hunter2");
    await user.type(repeatPassword, "Hunter2");
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/ошибка/i)).toBeInTheDocument();
    });
  });
});
