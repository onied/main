import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import ConfirmEmailComponent from "./confirmEmail";
import { MemoryRouter } from "react-router-dom";
import * as useProfileHook from "../../../hooks/profile/useProfile";
import { Profile } from "../../../hooks/profile/profile";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect, vitest } from "vitest";
import mockedNavigate from "../../../../test/mocks/useNavigate";
import backend from "../../../../test/helpers/backend";

describe("ConfirmEmailComponent", () => {
  it("loads and reports expired link when confirmEmail returns 401", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("Nothing", { status: 401 });
      })
    );
    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );
    // Act

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/.*истек.*/i)).toBeInTheDocument();
    });
  });

  it("redirects to login when required parameters are missing", async () => {
    // Arrange
    const url = `/confirmEmail`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("Nothing", { status: 401 });
      })
    );

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );
    // Act

    // Assert
    expect(mockedNavigate).toBeCalledWith(expect.stringContaining("/login"));
  });

  it("redirects to login when profile is missing", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [null, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("Nothing", { status: 401 });
      })
    );

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );
    // Act

    // Assert
    expect(mockedNavigate).toBeCalledWith(expect.stringContaining("/login"));
  });

  it("redirects to login when manage/2fa reports that token is incorrect", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("", { status: 200 });
      }),
      http.post(backend("/manage/2fa"), () => {
        return new HttpResponse("", { status: 404 });
      })
    );

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );
    // Act

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(
        expect.stringContaining("/login")
      );
    });
  });

  it("loads and confirms email", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    const sharedKey =
      "SXMgdGhpcyB0aGUgcmVhbCBsaWZlCklzIHRoaXMganVzdCBmYW50YXN5CkNhdWdodCBpbiBhIGxhbmRzbGlkZQpObyBlc2NhcGUgZnJvbSByZWFsaXR5";
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("", { status: 200 });
      })
    );
    server.use(
      http.post(backend("/manage/2fa"), () => {
        return HttpResponse.json({ sharedKey: sharedKey });
      })
    );

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );
    // Act

    // Assert
    expect(screen.getByText(/.*почта.*подтверждена/i)).toBeInTheDocument();
  });

  it("shows key", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    const sharedKey =
      "SXMgdGhpcyB0aGUgcmVhbCBsaWZlCklzIHRoaXMganVzdCBmYW50YXN5CkNhdWdodCBpbiBhIGxhbmRzbGlkZQpObyBlc2NhcGUgZnJvbSByZWFsaXR5";
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("", { status: 200 });
      })
    );
    server.use(
      http.post(backend("/manage/2fa"), () => {
        return HttpResponse.json({ sharedKey: sharedKey });
      })
    );
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );

    // Act
    await user.click(screen.getByText(/показать ключ/i));

    // Assert
    expect(screen.getByText(sharedKey)).toBeInTheDocument();
  });

  it("sends 2fa correctly and redirects to main", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    const sharedKey =
      "SXMgdGhpcyB0aGUgcmVhbCBsaWZlCklzIHRoaXMganVzdCBmYW50YXN5CkNhdWdodCBpbiBhIGxhbmRzbGlkZQpObyBlc2NhcGUgZnJvbSByZWFsaXR5";
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("", { status: 200 });
      })
    );
    server.use(
      http.post(backend("/manage/2fa"), async ({ request }) => {
        const data: any = (await request.json()) ?? { enable: false };
        if (data.enable && data.twoFactorCode === "123456") {
          return new HttpResponse("", { status: 200 });
        }
        return HttpResponse.json({ sharedKey: sharedKey });
      })
    );
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );

    // Act
    await user.keyboard("1{tab}2{tab}3{tab}4{tab}5{tab}6{tab}");
    await user.click(screen.getByText(/подтвердить/i));

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith("/");
    });
  });

  it("displays an error when server returns error", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    const sharedKey =
      "SXMgdGhpcyB0aGUgcmVhbCBsaWZlCklzIHRoaXMganVzdCBmYW50YXN5CkNhdWdodCBpbiBhIGxhbmRzbGlkZQpObyBlc2NhcGUgZnJvbSByZWFsaXR5";
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("", { status: 200 });
      })
    );
    server.use(
      http.post(backend("/manage/2fa"), async ({ request }) => {
        const data: any = (await request.json()) ?? { enable: false };
        if (data.enable) {
          return new HttpResponse("", { status: 500 });
        }
        return HttpResponse.json({ sharedKey: sharedKey });
      })
    );
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );

    // Act
    await user.keyboard("1{tab}2{tab}3{tab}4{tab}5{tab}6{tab}");
    await user.click(screen.getByText(/подтвердить/i));

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/ошибка сервера/i)).toBeInTheDocument();
    });
  });

  it("displays an error when invalid 2fa has been sent", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    const sharedKey =
      "SXMgdGhpcyB0aGUgcmVhbCBsaWZlCklzIHRoaXMganVzdCBmYW50YXN5CkNhdWdodCBpbiBhIGxhbmRzbGlkZQpObyBlc2NhcGUgZnJvbSByZWFsaXR5";
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("", { status: 200 });
      })
    );
    server.use(
      http.post(backend("/manage/2fa"), async ({ request }) => {
        const data: any = (await request.json()) ?? { enable: false };
        if (data.enable) {
          if (data.twoFactorCode === "123456")
            return new HttpResponse("", { status: 200 });
          else return new HttpResponse("", { status: 400 });
        }
        return HttpResponse.json({ sharedKey: sharedKey });
      })
    );
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );

    // Act
    await user.keyboard("1{tab}2{tab}3{tab}4{tab}5{tab}7{tab}");
    await user.click(screen.getByText(/подтвердить/i));

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/неверный код/i)).toBeInTheDocument();
    });
  });

  it("displays an error when 2fa not present", async () => {
    // Arrange
    const userId = "d31d55e0-f0b6-4804-b56f-cf283afb685d";
    const code = "QWxsIHlvdXIgYmFzZSBhcmUgYmVsb25nIHRvIHVz";
    const url = `/confirmEmail?userId=${userId}&code=${code}`;
    const profile: Profile = {
      firstName: "Ivan",
      lastName: "Ivanov",
      gender: 0,
      avatar: null,
      email: "ivan@ivanov.ru",
    };
    const sharedKey =
      "SXMgdGhpcyB0aGUgcmVhbCBsaWZlCklzIHRoaXMganVzdCBmYW50YXN5CkNhdWdodCBpbiBhIGxhbmRzbGlkZQpObyBlc2NhcGUgZnJvbSByZWFsaXR5";
    vitest.spyOn(useProfileHook, "useProfile").mockImplementation(() => {
      return [profile, false];
    });
    server.use(
      http.get(backend("/confirmEmail"), () => {
        return new HttpResponse("", { status: 200 });
      })
    );
    server.use(
      http.post(backend("/manage/2fa"), async ({ request }) => {
        const data: any = (await request.json()) ?? { enable: false };
        if (data.enable) {
          if (data.twoFactorCode === "123456")
            return new HttpResponse("", { status: 200 });
          else return new HttpResponse("", { status: 400 });
        }
        return HttpResponse.json({ sharedKey: sharedKey });
      })
    );
    const user = userEvent.setup();

    render(
      <MemoryRouter initialEntries={[url]}>
        <ConfirmEmailComponent />
      </MemoryRouter>
    );

    // Act
    await user.click(screen.getByText(/подтвердить/i));

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/не заполнено/i)).toBeInTheDocument();
    });
  });
});
