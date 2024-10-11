import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import RegistrationForm from "./registrationForm";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../../test/mocks/server";
import { describe, it, expect, test } from "vitest";
import backend from "../../../../../test/helpers/backend";
import mockedNavigate from "../../../../../test/mocks/useNavigate";

describe("RegistrationForm", () => {
  it("registers when everything is correct", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/register"]}>
        <RegistrationForm />
      </MemoryRouter>
    );
    const firstNameInput = screen.getByPlaceholderText(/имя/i);
    const lastNameInput = screen.getByPlaceholderText(/фамилия/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/^пароль/i);
    const repeatPasswordInput =
      screen.getByPlaceholderText(/(повтор|еще раз)/i);
    // const genderNotSpecified = screen.getByLabelText(/не указан/i);
    const genderMale = screen.getByLabelText(/муж/i);
    // const genderFemale = screen.getByLabelText(/жен/i);
    const button = screen.getByText(/далее/i);
    const user = userEvent.setup();
    server.use(
      http.post(backend("/register"), async ({ request }) => {
        const data: any = await request.json();
        if (data.email == "asdf@asdf.asdf") return HttpResponse.json();
        return new HttpResponse(null, { status: 400 });
      })
    );

    // Act
    await user.type(firstNameInput, "Ivan");
    await user.type(lastNameInput, "Ivanov");
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "hunter2");
    await user.type(repeatPasswordInput, "hunter2");
    await user.click(genderMale);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/login");
    });
  });
  it("reports when email is already used", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/register"]}>
        <RegistrationForm />
      </MemoryRouter>
    );
    const firstNameInput = screen.getByPlaceholderText(/имя/i);
    const lastNameInput = screen.getByPlaceholderText(/фамилия/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/^пароль/i);
    const repeatPasswordInput =
      screen.getByPlaceholderText(/(повтор|еще раз)/i);
    // const genderNotSpecified = screen.getByLabelText(/не указан/i);
    const genderMale = screen.getByLabelText(/муж/i);
    // const genderFemale = screen.getByLabelText(/жен/i);
    const button = screen.getByText(/далее/i);
    const user = userEvent.setup();
    server.use(
      http.post(backend("/register"), async ({ request }) => {
        const data: any = await request.json();
        if (data.email == "asdf@asdf.asdf")
          return HttpResponse.json(
            { errors: { DuplicateUserName: "" } },
            { status: 400 }
          );
        return new HttpResponse(null, { status: 400 });
      })
    );

    // Act
    await user.type(firstNameInput, "Ivan");
    await user.type(lastNameInput, "Ivanov");
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "Hunter2!");
    await user.type(repeatPasswordInput, "Hunter2!");
    await user.click(genderMale);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/уже.*используется/i)).toBeInTheDocument();
    });
  });
  it("reports when password too easy comes from backend", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/register"]}>
        <RegistrationForm />
      </MemoryRouter>
    );
    const firstNameInput = screen.getByPlaceholderText(/имя/i);
    const lastNameInput = screen.getByPlaceholderText(/фамилия/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/^пароль/i);
    const repeatPasswordInput =
      screen.getByPlaceholderText(/(повтор|еще раз)/i);
    // const genderNotSpecified = screen.getByLabelText(/не указан/i);
    const genderMale = screen.getByLabelText(/муж/i);
    // const genderFemale = screen.getByLabelText(/жен/i);
    const button = screen.getByText(/далее/i);
    const user = userEvent.setup();
    server.use(
      http.post(backend("/register"), async ({ request }) => {
        const data: any = await request.json();
        if (data.email == "asdf@asdf.asdf")
          return HttpResponse.json(
            { errors: { PasswordShouldContain: "" } },
            { status: 400 }
          );
        return new HttpResponse(null, { status: 400 });
      })
    );

    // Act
    await user.type(firstNameInput, "Ivan");
    await user.type(lastNameInput, "Ivanov");
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "Hunter2!");
    await user.type(repeatPasswordInput, "Hunter2!");
    await user.click(genderMale);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/(простой|легкий)/i)).toBeInTheDocument();
    });
  });
  it("reports when passwords don't match", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/register"]}>
        <RegistrationForm />
      </MemoryRouter>
    );
    const firstNameInput = screen.getByPlaceholderText(/имя/i);
    const lastNameInput = screen.getByPlaceholderText(/фамилия/i);
    const emailInput = screen.getByPlaceholderText(/адрес/i);
    const passwordInput = screen.getByPlaceholderText(/^пароль/i);
    const repeatPasswordInput =
      screen.getByPlaceholderText(/(повтор|еще раз)/i);
    // const genderNotSpecified = screen.getByLabelText(/не указан/i);
    const genderMale = screen.getByLabelText(/муж/i);
    // const genderFemale = screen.getByLabelText(/жен/i);
    const button = screen.getByText(/далее/i);
    const user = userEvent.setup();
    server.use(
      http.post(backend("/register"), async ({ request }) => {
        const data: any = await request.json();
        if (data.email == "asdf@asdf.asdf")
          return HttpResponse.json(
            { errors: { PasswordShouldContain: "" } },
            { status: 400 }
          );
        return new HttpResponse(null, { status: 400 });
      })
    );

    // Act
    await user.type(firstNameInput, "Ivan");
    await user.type(lastNameInput, "Ivanov");
    await user.type(emailInput, "asdf@asdf.asdf");
    await user.type(passwordInput, "Hunter2!");
    await user.type(repeatPasswordInput, "Hunter2");
    await user.click(genderMale);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/не.*совпад/i)).toBeInTheDocument();
    });
  });
  test.each([
    ["", "lastName", "email", /имя/i],
    ["firstName", "", "email", /фамил/i],
    ["firstName", "lastName", "", /адрес/i],
  ])(
    "reports when one of the basic fields is missing",
    async (firstName, lastName, email, expectedRegex) => {
      // Arrange
      render(
        <MemoryRouter initialEntries={["/register"]}>
          <RegistrationForm />
        </MemoryRouter>
      );
      const firstNameInput = screen.getByPlaceholderText(/имя/i);
      const lastNameInput = screen.getByPlaceholderText(/фамилия/i);
      const emailInput = screen.getByPlaceholderText(/адрес/i);
      emailInput.removeAttribute("type");
      const passwordInput = screen.getByPlaceholderText(/^пароль/i);
      const repeatPasswordInput =
        screen.getByPlaceholderText(/(повтор|еще раз)/i);
      // const genderNotSpecified = screen.getByLabelText(/не указан/i);
      const genderMale = screen.getByLabelText(/муж/i);
      // const genderFemale = screen.getByLabelText(/жен/i);
      const button = screen.getByText(/далее/i);
      const user = userEvent.setup();
      server.use(
        http.post(backend("/register"), async () => {
          return HttpResponse.json({ errors: {} }, { status: 400 });
        })
      );

      // Act
      if (firstName) await user.type(firstNameInput, firstName);
      if (lastName) await user.type(lastNameInput, lastName);
      if (email) await user.type(emailInput, email);
      await user.type(passwordInput, "Hunter2!");
      await user.type(repeatPasswordInput, "Hunter2!");
      await user.click(genderMale);
      await user.click(button);

      // Assert
      await waitFor(() => {
        expect(screen.getByText(expectedRegex)).toBeInTheDocument();
      });
    }
  );
  test.each(["hunter", "as", "h!unter", "1jkl*", "!Hunter", "!hunter2"])(
    "reports when password too easy",
    async (password) => {
      // Arrange
      render(
        <MemoryRouter initialEntries={["/register"]}>
          <RegistrationForm />
        </MemoryRouter>
      );
      const firstNameInput = screen.getByPlaceholderText(/имя/i);
      const lastNameInput = screen.getByPlaceholderText(/фамилия/i);
      const emailInput = screen.getByPlaceholderText(/адрес/i);
      const passwordInput = screen.getByPlaceholderText(/^пароль/i);
      const repeatPasswordInput =
        screen.getByPlaceholderText(/(повтор|еще раз)/i);
      // const genderNotSpecified = screen.getByLabelText(/не указан/i);
      const genderMale = screen.getByLabelText(/муж/i);
      // const genderFemale = screen.getByLabelText(/жен/i);
      const button = screen.getByText(/далее/i);
      const user = userEvent.setup();
      server.use(
        http.post(backend("/register"), async () => {
          return HttpResponse.json({ errors: {} }, { status: 400 });
        })
      );

      // Act
      await user.type(firstNameInput, "Ivan");
      await user.type(lastNameInput, "Ivanov");
      await user.type(emailInput, "asdf@asdf.asdf");
      await user.type(passwordInput, password);
      await user.type(repeatPasswordInput, password);
      await user.click(genderMale);
      await user.click(button);

      // Assert
      await waitFor(() => {
        expect(screen.getByText(/(простой|легкий)/i)).toBeInTheDocument();
      });
    }
  );
});
