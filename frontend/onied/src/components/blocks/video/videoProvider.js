/**
 * Abstract Class VideoProvider.
 *
 * @class VideoProvider
 */
export default class VideoProvider {
  constructor() {
    if (this.constructor == VideoProvider) {
      throw new Error("Abstract classes can't be instantiated.");
    }
  }

  // regex: регулярное выражение, соответствующее видеопровайдеру.
  regex = new RegExp();

  // getIFrameLink: функция, которая конвертирует ссылку в ссылку для iframe.
  // regex должен содержать именованные группы, содержащиеся в convertHrefToIFrameSrc.
  getLink(href) {
    throw new Error("Method 'getLink()' must be implemented.");
  }
}
