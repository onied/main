import "@testing-library/jest-dom";
import {
  render,
  screen,
  waitFor,
  waitForElementToBeRemoved,
} from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import OrderCertificate from "./orderCertificate";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect, vi } from "vitest";
import backend from "../../../../test/helpers/backend";
import mockedNavigate from "../../../../test/mocks/useNavigate";
import * as useProfileHook from "../../../hooks/profile/useProfile";
import { Profile } from "../../../hooks/profile/profile";

vi.mock("@mapbox/search-js-react", () => {
  return {
    SearchBox: ({ value, onChange }: any) => (
      <input value={value} onChange={(e) => onChange(e.target.value)} />
    ),
  };
});
vi.mock("react-map-gl", () => {
  return {
    default: ({ children }: any) => <div>{children}</div>,
  };
});

describe("OrderCertificate", () => {
  it("reports when user didn't write address", async () => {
    // Arrange
    const address = "1 Apple Park Way, Cupertino, CA 95014, USA";
    server.use(
      http.get(backend("/certificates/1"), () => {
        return HttpResponse.json({
          price: 1000,
          course: {
            title: "asdf",
            author: { firstName: "Vasiliy", lastName: "Petrovich" },
          },
        });
      }),
      http.get("https://api.mapbox.com/search/geocode/v6/forward", () => {
        return HttpResponse.json({
          features: {
            properties: { feature_type: "address", full_address: address },
          },
        });
      })
    );
    render(
      <MemoryRouter initialEntries={["/1"]}>
        <Routes>
          <Route path="/:courseId" element={<OrderCertificate />} />
        </Routes>
      </MemoryRouter>
    );
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
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^заказать$/i);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/укажите адрес/i)).toBeInTheDocument();
    });
  });
  it("reports when backend returned errors", async () => {
    // Arrange
    const address = "1 Apple Park Way, Cupertino, CA 95014, USA";
    server.use(
      http.get(backend("/certificates/1"), () => {
        return new HttpResponse(null, { status: 400 });
      }),
      http.get("https://api.mapbox.com/search/geocode/v6/forward", () => {
        return HttpResponse.json({
          features: {
            properties: { feature_type: "address", full_address: address },
          },
        });
      })
    );
    render(
      <MemoryRouter initialEntries={["/1"]}>
        <Routes>
          <Route path="/:courseId" element={<OrderCertificate />} />
        </Routes>
      </MemoryRouter>
    );
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

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/ошибка/i)).toBeInTheDocument();
    });
  });
  it("reports when id not a number", async () => {
    // Arrange
    const address = "1 Apple Park Way, Cupertino, CA 95014, USA";
    server.use(
      http.get(backend("/certificates/1"), () => {
        return new HttpResponse(null, { status: 400 });
      }),
      http.get("https://api.mapbox.com/search/geocode/v6/forward", () => {
        return HttpResponse.json({
          features: {
            properties: { feature_type: "address", full_address: address },
          },
        });
      })
    );
    render(
      <MemoryRouter initialEntries={["/asdf"]}>
        <Routes>
          <Route path="/:courseId" element={<OrderCertificate />} />
        </Routes>
      </MemoryRouter>
    );
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

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.getByText(/ошибка/i)).toBeInTheDocument();
    });
  });
  it("orders certificate", async () => {
    // Arrange
    const address = "1 Apple Park Way, Cupertino, CA 95014, USA";
    server.use(
      http.get(backend("/certificates/1"), () => {
        return HttpResponse.json({
          price: 1000,
          course: {
            title: "asdf",
            author: { firstName: "Vasiliy", lastName: "Petrovich" },
          },
        });
      }),
      http.get("https://api.mapbox.com/search/geocode/v6/forward", () => {
        return HttpResponse.json({
          type: "FeatureCollection",
          features: [
            {
              properties: { feature_type: "address", full_address: address },
            },
          ],
        });
      })
    );
    render(
      <MemoryRouter initialEntries={["/1"]}>
        <Routes>
          <Route path="/:courseId" element={<OrderCertificate />} />
        </Routes>
      </MemoryRouter>
    );
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
    const user = userEvent.setup();

    // Act
    const button = await screen.findByText(/^заказать$/i);
    await user.keyboard("{tab}" + address);
    await user.click(button);

    // Assert
    await waitFor(() => {
      expect(mockedNavigate).toHaveBeenCalledWith(
        "/purchases/certificate/1",
        expect.objectContaining({
          state: {
            address: address,
          },
        })
      );
    });
  });
});
