import VideoProvider from "./videoProvider";

/**
 * VK Video Provider.
 *
 * @class VkVideoProvider
 * @extends {VideoProvider}
 */
export default class VkVideoProvider extends VideoProvider {
  rawVideo = false;
  regex =
    /^(?:https:\/\/)?(?:www\.)?(?:vk\.com|m\.vk\.com)(?:\/video)(?<videoOid>[\d-]+)_(?<videoId>[\d]+)$/;

  getLink(href: string) {
    const matches = href.match(this.regex);

    return Promise.resolve(
      `https://vk.com/video_ext.php?oid=${matches?.groups?.videoOid}&id=${matches?.groups?.videoId}`
    );
  }
}
