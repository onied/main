import api from "@onied/config/axios";
import VideoProvider from "./videoProvider";

/**
 * File Video Provider.
 *
 * @class FileVideoProvider
 * @extends {VideoProvider}
 */
export default class FileVideoProvider extends VideoProvider {
  regex = /^[a-z0-9_\-]+$/;

  async getLink(href: string) {
    const response = await api.get("/storage/download-url/" + href);
    return response.data.presignedUrl;
  }
}
