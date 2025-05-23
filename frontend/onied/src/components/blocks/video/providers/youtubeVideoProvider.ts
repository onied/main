import VideoProvider from "./videoProvider";

/**
 * Youtube Video Provider.
 *
 * @class YoutubeVideoProvider
 * @extends {VideoProvider}
 */
export default class YoutubeVideoProvider extends VideoProvider {
  rawVideo = false;
  regex =
    /^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w]+)(\S+)?$/;

  getLink(href: string) {
    const matches = href.match(this.regex);

    return Promise.resolve(
      `https://www.youtube.com/embed/${matches?.groups?.videoId}`
    );
  }
}
