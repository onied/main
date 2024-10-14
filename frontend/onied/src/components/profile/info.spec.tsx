import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import ProfileInfo from "./info";
import { http, HttpResponse } from "msw";
import { server } from "../../../test/mocks/server";
import { describe, it, expect, vi } from "vitest";
import backend from "../../../test/helpers/backend";
import * as useProfileHook from "../../hooks/profile/useProfile";
import { MemoryRouter } from "react-router-dom";
import { Profile } from "../../hooks/profile/profile";
import mockedNavigate from "../../../test/mocks/useNavigate";

describe("ProfileInfo", () => {
  it("logs out", async () => {
    // Arrange
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vi.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.post(backend("/forgotPassword"), () => HttpResponse.json()),
      http.put(backend("/profile/avatar"), () => HttpResponse.json()),
      http.put(backend("/profile"), () => HttpResponse.json())
    );
    render(
      <MemoryRouter>
        <ProfileInfo />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const logout = await screen.findByText(/выйти/i);
    await user.click(logout);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/");
    });
  });
  it("sends password reset", async () => {
    // Arrange
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vi.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.post(backend("/forgotPassword"), () => HttpResponse.json()),
      http.put(backend("/profile/avatar"), () => HttpResponse.json()),
      http.put(backend("/profile"), () => HttpResponse.json())
    );
    render(
      <MemoryRouter>
        <ProfileInfo />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/сменить пароль/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/отправлено/i)).toBeInTheDocument();
    });
  });
  it("deletes avatar", async () => {
    // Arrange
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: "https://example.com",
      email: "ivan@ivanov.ru",
    };
    vi.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.post(backend("/forgotPassword"), () => HttpResponse.json()),
      http.put(backend("/profile/avatar"), () => HttpResponse.json()),
      http.put(backend("/profile"), () => HttpResponse.json())
    );
    render(
      <MemoryRouter>
        <ProfileInfo />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/удалить/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/загрузить аватар/i)).toBeNull();
    });
  });
  it("changes avatar", async () => {
    // Arrange
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vi.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.post(backend("/forgotPassword"), () => HttpResponse.json()),
      http.put(backend("/profile/avatar"), () => HttpResponse.json()),
      http.put(backend("/profile"), () => HttpResponse.json())
    );
    render(
      <MemoryRouter>
        <ProfileInfo />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const buttonDialog = await screen.findByText(/загрузить/i);
    await user.click(buttonDialog);
    const link = await screen.findByPlaceholderText(/ссылка/i);
    await user.type(link, "http://example.com");
    const button = await screen.findByText(/сохранить аватар/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/загрузить аватар/i)).toBeNull();
    });
  });
  it("reports invalid url when changing avatar", async () => {
    // Arrange
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vi.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.post(backend("/forgotPassword"), () => HttpResponse.json()),
      http.put(backend("/profile/avatar"), () =>
        HttpResponse.json(
          {
            errors: {
              AvatarHref: true,
            },
          },
          { status: 400 }
        )
      ),
      http.put(backend("/profile"), () => HttpResponse.json())
    );
    render(
      <MemoryRouter>
        <ProfileInfo />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const buttonDialog = await screen.findByText(/загрузить/i);
    await user.click(buttonDialog);
    const link = await screen.findByPlaceholderText(/ссылка/i);
    await user.type(link, "http://example.com");
    const button = await screen.findByText(/сохранить аватар/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/корректный url/i)).toBeInTheDocument();
    });
  });
  it("saves profile info", async () => {
    // Arrange
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vi.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.post(backend("/forgotPassword"), () => HttpResponse.json()),
      http.put(backend("/profile/avatar"), () => HttpResponse.json()),
      http.put(backend("/profile"), () => HttpResponse.json())
    );
    render(
      <MemoryRouter>
        <ProfileInfo />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const firstName = await screen.findByLabelText(/имя/i);
    await user.type(firstName, "Ivan");
    const lastName = await screen.findByLabelText(/фамилия/i);
    await user.type(lastName, "Ivanov");
    const genderMale = await screen.findByLabelText(/мужской/i);
    await user.click(genderMale);
    const genderFemale = await screen.findByLabelText(/женский/i);
    await user.click(genderFemale);
    const genderOther = await screen.findByLabelText(/не указан/i);
    await user.click(genderOther);

    const button = await screen.findByText(/^сохранить$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/сохранено/i)).toBeInTheDocument();
    });
  });
  it("reports errors in profile", async () => {
    // Arrange
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vi.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.post(backend("/forgotPassword"), () => HttpResponse.json()),
      http.put(backend("/profile/avatar"), () => HttpResponse.json()),
      http.put(backend("/profile"), () =>
        HttpResponse.json(
          {
            errors: {
              Gender: true,
              FirstName: true,
              LastName: true,
            },
          },
          { status: 400 }
        )
      )
    );
    render(
      <MemoryRouter>
        <ProfileInfo />
      </MemoryRouter>
    );
    const user = userEvent.setup();

    // Act
    const firstName = await screen.findByLabelText(/имя/i);
    await user.type(firstName, "Ivan");
    const lastName = await screen.findByLabelText(/фамилия/i);
    await user.type(lastName, "Ivanov");
    const genderMale = await screen.findByLabelText(/мужской/i);
    await user.click(genderMale);
    const genderFemale = await screen.findByLabelText(/женский/i);
    await user.click(genderFemale);
    const genderOther = await screen.findByLabelText(/не указан/i);
    await user.click(genderOther);

    const button = await screen.findByText(/^сохранить$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(/неверное значение/i)).toBeInTheDocument();
      expect(screen.queryByText(/правильное имя/i)).toBeInTheDocument();
      expect(screen.queryByText(/правильн.* фамили/i)).toBeInTheDocument();
    });
  });
});
