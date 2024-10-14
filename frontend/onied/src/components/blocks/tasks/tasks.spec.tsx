import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { describe, it, expect } from "vitest";

import { server } from "@onied/tests/mocks/server";
import backend from "@onied/tests/helpers/backend";
import { BlockType, TasksBlock } from "@onied/types/block";
import Tasks from "./tasks";

describe("Summary", () => {
    it("renders summary with no file when valid response", async () => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/summary/${blockId}`;

        const summary: TasksBlock = {
            id: blockId,
            title: "default text",
            blockType: BlockType.SummaryBlock,
            markdownText: "шаблонный текст",
            completed: false
        }

        server.use(
            http.get(backend(url), () => {
                return HttpResponse.json(summary, { status: 200 });
            })
        );

        // render(
        //     <MemoryRouter initialEntries={[url]}>
        //         <Summary courseId={courseId} blockId={blockId} />
        //     </MemoryRouter>
        // );

        // await waitFor(() => {
        //     expect(screen.getByTestId('markdown-summary-title')).toBeInTheDocument();
        //     expect(screen.queryByTestId('markdown-summary-file-link')).not.toBeInTheDocument();
        //     expect(screen.getByText(/шаблонный текст/i)).toBeInTheDocument();
        // });
    });
})