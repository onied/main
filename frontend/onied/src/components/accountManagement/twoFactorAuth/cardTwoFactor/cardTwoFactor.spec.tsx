import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import CardTwoFactor from "./cardTwoFactor";
import { MemoryRouter } from "react-router-dom";
import { describe, it, expect } from "vitest";
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

  it("redirects to login when 2fa was typed", async () => {
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
          state: expect.objectContaining({
            email: "asdf@asdf.asdf",
            password: "hunter2",
            twoFactorCode: "123456",
          }),
        })
      );
    });
  });
  it("redirects to login when 2fa was typed and passes redirect", async () => {
    // Arrange
    render(
      <MemoryRouter
        initialEntries={[
          {
            pathname: "/login/2fa",
            state: {
              email: "asdf@asdf.asdf",
              password: "hunter2",
              redirect: "b83b87fe-05a2-411b-a5a0-d1e3384ff5cf",
            },
          },
        ]}
      >
        <CardTwoFactor />
      </MemoryRouter>
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
          state: expect.objectContaining({
            email: "asdf@asdf.asdf",
            password: "hunter2",
            twoFactorCode: "123456",
            redirect: "b83b87fe-05a2-411b-a5a0-d1e3384ff5cf",
          }),
        })
      );
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

    const button = screen.getByText(/подтвердить/i);
    const user = userEvent.setup();

    // Act
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/не заполнен/i)).toBeInTheDocument();
    });
  });
});
