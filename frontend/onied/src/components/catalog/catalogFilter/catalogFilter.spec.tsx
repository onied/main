import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import CatalogFilter from "./catalogFilter";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { server } from "../../../../test/mocks/server";
import { describe, it, expect } from "vitest";
import backend from "../../../../test/helpers/backend";

describe("CatalogFilter", () => {
  it("renders correctly", async () => {
    // Arrange
    render(
      <MemoryRouter initialEntries={["/catalog"]}>
        <CatalogFilter />
      </MemoryRouter>
    );
    server.use(
      http.get(backend("/categories"), () => {
        return HttpResponse.json([{ id: 1, name: "Vasiliy" }]);
      })
    );

    // Act

    // Assert
    expect(screen.getByLabelText(/сертификат/i)).not.toBeChecked();
    expect(screen.getByLabelText(/актив/i)).not.toBeChecked();
  });
});
