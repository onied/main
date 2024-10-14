import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import NotificationContainer from "./index";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect, vi } from "vitest";
import backend from "../../../../test/helpers/backend";
import * as useSignalR from "../../../hooks/signalr";
import { Notification } from "../../../types/notifications";

describe("NotificationContainer", () => {
  it("renders without notifications", async () => {
    // Arrange
    const notifications = [
      {
        id: 1,
        img: "",
        title: "string",
        message: "string",
        isRead: false,
      },
    ];
    server.use(
      http.get(backend("/notifications"), () => {
        return HttpResponse.json(notifications);
      })
    );
    render(<NotificationContainer />);

    // Act

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(notifications[0].message)).toBeNull();
    });
  });
  it("renders notifications", async () => {
    // Arrange
    const notifications = [
      {
        id: 1,
        img: "",
        title: "string",
        message: "asdfasdfasdfasfasdf",
        isRead: false,
      },
    ];
    server.use(
      http.get(backend("/notifications"), () => {
        return HttpResponse.json(notifications);
      })
    );
    render(<NotificationContainer />);
    const user = userEvent.setup();

    // Act
    const bell = screen.getByAltText(/уведомления/i);
    await user.click(bell);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(notifications[0].message)).toBeInTheDocument();
    });
  });
  it("marks notifications as read", async () => {
    // Arrange
    const notifications = [
      {
        id: 1,
        img: "",
        title: "string",
        message: "asdfasdfasdfasfasdf",
        isRead: false,
      },
    ];
    server.use(
      http.get(backend("/notifications"), () => {
        return HttpResponse.json(notifications);
      })
    );
    const mockedSend = vi.fn();

    vi.spyOn(useSignalR, "default").mockImplementation(() => {
      return {
        connection: {
          on: vi.fn(),
          off: vi.fn(),
          send: mockedSend,
        } as any,
      };
    });
    render(<NotificationContainer />);
    const user = userEvent.setup();

    // Act
    const bell = screen.getByAltText(/уведомления/i);
    await user.click(bell);
    const readButton = screen.getByTestId("read-button");
    await user.click(readButton);

    // Assert
    await waitFor(() => {
      expect(screen.queryByText(notifications[0].message)).toBeInTheDocument();
      expect(readButton).not.toBeInTheDocument();
      expect(mockedSend).toHaveBeenCalledWith("UpdateRead", 1);
    });
  });
  it("adds notifications", async () => {
    // Arrange
    const notifications: Notification[] = [];
    server.use(
      http.get(backend("/notifications"), () => {
        return HttpResponse.json(notifications);
      })
    );
    const mockedOn = vi.fn();

    vi.spyOn(useSignalR, "default").mockImplementation(() => {
      return {
        connection: {
          on: mockedOn,
          off: vi.fn(),
          send: vi.fn(),
        } as any,
      };
    });
    render(<NotificationContainer />);
    const user = userEvent.setup();

    // Act
    const bell = screen.getByAltText(/уведомления/i);
    await user.click(bell);

    const send: (message: Notification) => void = mockedOn.mock.calls[0][1];
    send({
      id: 1,
      img: "",
      title: "string",
      message: "asdfasdfasdfasfasdf",
      isRead: false,
    });

    // Assert
    await waitFor(() => {
      expect(screen.queryByText("asdfasdfasdfasfasdf")).toBeInTheDocument();
    });
  });
});
