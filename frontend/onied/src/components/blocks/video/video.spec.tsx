import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { http, HttpResponse } from "msw";
import { describe, it, expect } from "vitest";

import { server } from "@onied/tests/mocks/server";
import backend from "@onied/tests/helpers/backend";
import { BlockType, VideoBlock } from "@onied/types/block";
import Video from "./video";

describe("Video", () => {
    test.each([
        "https://vk.com/video-220754053_456240120",
        "https://rutube.ru/video/c6cc4d620b1d4338901770a44b3e82f4/",
        "https://www.youtube.com/watch?v=cPCLFtxpadE",
        "https://youtu.be/dQw4w9WgXcQ?si=24ULVROXDJRLEDUT",
    ])("renders videoblock with valid link", async (videoUrl) => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/video/${blockId}`;
        
        const videoBlock: VideoBlock = {
            id: blockId,
            index: 1,
            title: "default video",
            blockType: BlockType.VideoBlock,
            completed: false,
            href: videoUrl
        };

        server.use(
            http.get(backend(url), () => {
                return HttpResponse.json(videoBlock, { status: 200 });
            })
        );

        render(
            <MemoryRouter initialEntries={[url]}>
                <Video courseId={courseId} blockId={blockId} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(screen.getByTestId('iframe-video')).toBeInTheDocument();
        });
    });

    test.each([
        "https://vk.com/video?q=never%20gonna%20give%20you%20up&z=video50861944_456251300%2Fpl_cat_trends",
    ])("throws error when invalid link", async (videoUrl) => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/video/${blockId}`;
        
        const videoBlock: VideoBlock = {
            id: blockId,
            index: 1,
            title: "default video",
            blockType: BlockType.VideoBlock,
            completed: false,
            href: videoUrl
        };

        server.use(
            http.get(backend(url), () => {
                return HttpResponse.json(videoBlock, { status: 200 });
            })
        );

        render(
            <MemoryRouter initialEntries={[url]}>
                <Video courseId={courseId} blockId={blockId} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(screen.getByText(/Неверный формат ссылки на видео/i)).toBeInTheDocument();
        });
    });

    it("doesn't render video when returns 404", async () => {
        const courseId = 1;
        const blockId = 1;
        const url = `courses/${courseId}/video/${blockId}`;

        server.use(
            http.get(backend(url), () => {
                return new HttpResponse("", { status: 404 });
            })
        );

        render(
            <MemoryRouter initialEntries={[url]}>
                <Video courseId={courseId} blockId={blockId} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(screen.queryByTestId('iframe-video')).not.toBeInTheDocument();
        });
    });
})