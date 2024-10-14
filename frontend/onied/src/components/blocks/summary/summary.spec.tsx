import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { describe, it, expect } from "vitest";

import { server } from "@onied/tests/mocks/server";
import backend from "@onied/tests/helpers/backend";
import { BlockType, SummaryBlock } from "@onied/types/block";
import Summary from "./summary";

describe("Summary", () => {
    it("renders summary with no file when valid response", async () => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/summary/${blockId}`;

        const summary: SummaryBlock = {
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

        render(
            <MemoryRouter initialEntries={[url]}>
                <Summary courseId={courseId} blockId={blockId} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(screen.getByTestId('markdown-summary-title')).toBeInTheDocument();
            expect(screen.queryByTestId('markdown-summary-file-link')).not.toBeInTheDocument();
            expect(screen.getByText(/шаблонный текст/i)).toBeInTheDocument();
        });
    });

    it("renders summary with file when valid response", async () => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/summary/${blockId}`;

        const summary: SummaryBlock = {
            id: blockId,
            title: "default text",
            blockType: BlockType.SummaryBlock,
            markdownText: "шаблонный текст",
            completed: false,
            fileName: 'filename.txt',
            fileHref: 'https://localhost/filename.txt'
        }

        server.use(
            http.get(backend(url), () => {
                return HttpResponse.json(summary, { status: 200 });
            })
        );

        render(
            <MemoryRouter initialEntries={[url]}>
                <Summary courseId={courseId} blockId={blockId} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(screen.getByTestId('markdown-summary-title')).toBeInTheDocument();
            expect(screen.getByTestId('markdown-summary-file-link')).toBeInTheDocument();
            expect(screen.getByText(/шаблонный текст/i)).toBeInTheDocument();
        });
    });

    it("renders summary with no file when no fileName in response", async () => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/summary/${blockId}`;

        const summary: SummaryBlock = {
            id: blockId,
            title: "default text",
            blockType: BlockType.SummaryBlock,
            markdownText: "шаблонный текст",
            completed: false,
            fileHref: 'https://localhost/filename.txt'
        }

        server.use(
            http.get(backend(url), () => {
                return HttpResponse.json(summary, { status: 200 });
            })
        );

        render(
            <MemoryRouter initialEntries={[url]}>
                <Summary courseId={courseId} blockId={blockId} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(screen.getByTestId('markdown-summary-title')).toBeInTheDocument();
            expect(screen.queryByTestId('markdown-summary-file-link')).not.toBeInTheDocument();
            expect(screen.getByText(/шаблонный текст/i)).toBeInTheDocument();
        });
    });

    it("renders summary with no file when no fileHref in response", async () => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/summary/${blockId}`;

        const summary: SummaryBlock = {
            id: blockId,
            title: "default text",
            blockType: BlockType.SummaryBlock,
            markdownText: "шаблонный текст",
            completed: false,
            fileName: 'filename.txt'
        }

        server.use(
            http.get(backend(url), () => {
                return HttpResponse.json(summary, { status: 200 });
            })
        );

        render(
            <MemoryRouter initialEntries={[url]}>
                <Summary courseId={courseId} blockId={blockId} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(screen.getByTestId('markdown-summary-title')).toBeInTheDocument();
            expect(screen.queryByTestId('markdown-summary-file-link')).not.toBeInTheDocument();
            expect(screen.getByText(/шаблонный текст/i)).toBeInTheDocument();
        });
    });
    
    it("doesn't render summary when returns 404", async () => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/summary/${blockId}`;

        server.use(
            http.get(backend(url), () => {
                return new HttpResponse("", { status: 404 });
            })
        );

        render(
            <MemoryRouter initialEntries={[url]}>
                <Summary courseId={courseId} blockId={blockId} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(screen.queryByTestId('markdown-summary-title')).not.toBeInTheDocument();
        });
    });
})