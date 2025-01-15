import VideoProvider from "./videoProvider";

/**
 * Rutube Video Provider.
 *
 * @class RutubeVideoProvider
 * @extends {VideoProvider}
 */
export default class RutubeVideoProvider extends VideoProvider {
  regex =
    /^((?:https?:)?\/\/)?(rutube\.ru)(\/video)(?<videoId>\/[\w\d]+)?(\/[\S]+)?$/;

  getLink(href: string) {
    const matches = href.match(this.regex);

    return Promise.resolve(
      `https://rutube.ru/play/embed/${matches?.groups?.videoId}`
    );
  }
}
