/**
 * Abstract Class VideoProvider.
 *
 * @class VideoProvider
 */
export default abstract class VideoProvider {

  // regex: регулярное выражение, соответствующее видеопровайдеру.
  abstract regex: RegExp;

  // getLink: функция, которая конвертирует ссылку в ссылку для iframe.
  // regex должен содержать именованные группы, содержащиеся в convertHrefToIFrameSrc.
  abstract getLink(href: string): string;
}
