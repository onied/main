import { http, HttpResponse } from "msw";
import { server } from "../../test/mocks/server";
import api from "../config/axios";
import LoginService from "./loginService";
import { describe, it, vi } from "vitest";
import backend from "../../test/helpers/backend";
import { waitFor } from "@testing-library/react";

describe("LoginService", () => {
  describe("initialize", () => {
    it("sets authorization header when accessToken provided", () => {
      // Arrange
      vi.spyOn(Storage.prototype, "getItem").mockImplementationOnce(
        () => "token"
      );

      // Act
      LoginService.initialize(vi.fn());

      // Assert
      expect(api.defaults.headers.common.Authorization).toMatch(/Bearer token/);
    });
  });
  describe("storeTokens", () => {
    it("stores correct information", () => {
      // Arrange
      vi.spyOn(Storage.prototype, "setItem");

      // Act
      LoginService.storeTokens("access_token", 10, "refresh_token");

      // Assert
      expect(api.defaults.headers.common.Authorization).toMatch(
        /Bearer access_token/
      );
      expect(localStorage.setItem).toHaveBeenNthCalledWith(
        1,
        "access_token",
        "access_token"
      );
      expect(localStorage.setItem).toHaveBeenNthCalledWith(
        2,
        "refresh_token",
        "refresh_token"
      );
      expect(localStorage.setItem).toHaveBeenCalledTimes(3);
    });
  });
  describe("refreshTokens", () => {
    it("stores correct information", async () => {
      // Arrange
      vi.spyOn(Storage.prototype, "setItem");
      vi.spyOn(Storage.prototype, "getItem").mockImplementationOnce(
        () => "token"
      );
      LoginService.setRefreshingTokens = vi.fn();
      server.use(
        http.post(backend("/refresh"), async ({ request }) => {
          const data: any = await request.json();
          if (data.refreshToken == "token")
            return HttpResponse.json({
              accessToken: "access_token",
              refreshToken: "refresh_token",
              expiresIn: 10,
            });
        })
      );

      // Act
      LoginService.refreshTokens();

      // Assert
      await waitFor(() => {
        expect(api.defaults.headers.common.Authorization).toMatch(
          /Bearer access_token/
        );
        expect(localStorage.setItem).toHaveBeenNthCalledWith(
          1,
          "access_token",
          "access_token"
        );
        expect(localStorage.setItem).toHaveBeenNthCalledWith(
          2,
          "refresh_token",
          "refresh_token"
        );
        expect(localStorage.setItem).toHaveBeenCalledTimes(3);
        expect(LoginService.setRefreshingTokens).toHaveBeenLastCalledWith(
          false
        );
      });
    });
    it("clears tokens when expired", async () => {
      // Arrange
      vi.spyOn(Storage.prototype, "removeItem");
      vi.spyOn(Storage.prototype, "getItem").mockImplementationOnce(
        () => "token"
      );
      server.use(
        http.post(backend("/refresh"), () => {
          return HttpResponse.json({}, { status: 401 });
        })
      );

      // Act
      LoginService.refreshTokens();

      // Assert
      await waitFor(() => {
        expect(localStorage.removeItem).toHaveBeenCalledTimes(3);
        expect(api.defaults.headers.common.Authorization).toBeUndefined();
      });
    });
    it("does not clear tokens when there's no internet", async () => {
      // Arrange
      vi.spyOn(Storage.prototype, "removeItem");
      vi.spyOn(Storage.prototype, "getItem").mockImplementationOnce(
        () => "token"
      );

      // Act
      LoginService.refreshTokens();

      // Assert
      await waitFor(() => {
        expect(localStorage.removeItem).not.toHaveBeenCalled();
      });
    });
  });
});
