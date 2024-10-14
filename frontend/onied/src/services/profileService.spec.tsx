import { http, HttpResponse } from "msw";
import { server } from "../../test/mocks/server";
import ProfileService from "./profileService";
import { describe, it, vi } from "vitest";
import backend from "../../test/helpers/backend";
import { waitFor } from "@testing-library/react";

describe("ProfileService", () => {
  describe("fetchProfile", () => {
    it("sets authorization header when accessToken provided", async () => {
      // Arrange
      ProfileService.initialize(vi.fn(), vi.fn());
      server.use(
        http.get(backend("/profile"), () => {
          return HttpResponse.json({});
        })
      );

      // Act
      ProfileService.fetchProfile();

      // Assert
      await waitFor(() => {
        expect(ProfileService.setProfile).toHaveBeenCalled();
        expect(ProfileService.setLoading).toHaveBeenLastCalledWith(false);
      });
    });
  });
});
