export interface Field {
  name: string;
  label: string;
  validRegex: string;
}

export interface MetadataContext {
  types: string[];
  fields: Field[];
}

const booksContext: MetadataContext = {
  types: [
    "application/pdf",
    "application/epub+zip",
    "image/vnd.djvu",
    "application/x-mobipocket-ebook",
    ".pdf",
    ".epub",
    ".djvu",
    ".mobi",
  ],
  fields: [
    { name: "Title", label: "Название", validRegex: "^.{0,50}$" },
    { name: "Description", label: "Описание", validRegex: "^.{0,100}$" },
    { name: "Author", label: "Автор", validRegex: "^.{0,50}$" },
    { name: "Publisher", label: "Издательство", validRegex: "^.{0,50}$" },
    { name: "Isbn", label: "ISBN", validRegex: "^\\d{10,13}$" },
  ],
};

const documentsContext: MetadataContext = {
  types: [
    "application/msword",
    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    "text/plain",
    "application/vnd.openxmlformats-officedocument.presentationml.presentation",
    "text/markdown",
    ".doc",
    ".docx",
    ".txt",
    ".pptx",
    ".md",
  ],
  fields: [
    { name: "Title", label: "Название", validRegex: "^.{0,50}$" },
    { name: "Description", label: "Описание", validRegex: "^.{0,100}$" },
    { name: "Author", label: "Автор", validRegex: "^.{0,50}$" },
  ],
};

const exerciseMaterialsContext: MetadataContext = {
  types: [
    "application/vnd.ms-excel",
    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    "text/csv",
    "application/zip",
    "application/x-rar-compressed",
    ".xls",
    ".xlsx",
    ".csv",
    ".zip",
    ".rar",
  ],
  fields: [
    { name: "Title", label: "Название", validRegex: "^.{0,50}$" },
    { name: "Description", label: "Описание", validRegex: "^.{0,100}$" },
  ],
};

const videosContext: MetadataContext = {
  types: [
    "video/mp4",
    "video/quicktime",
    "video/x-ms-wmv",
    "video/x-msvideo",
    "video/x-matroska",
    "video/webm",
    ".mp4",
    ".mov",
    ".wmv",
    ".avi",
    ".mkv",
    ".webm",
  ],
  fields: [
    { name: "Title", label: "Название", validRegex: "^.{0,50}$" },
    { name: "Description", label: "Описание", validRegex: "^.{0,100}$" },
    { name: "Subject", label: "Тема", validRegex: "^.{0,50}$" },
    { name: "Author", label: "Автор", validRegex: "^.{0,50}$" },
  ],
};

const audioContext: MetadataContext = {
  types: [
    "audio/mpeg",
    "audio/wav",
    "audio/x-flac",
    "audio/ogg",
    "audio/x-ms-wma",
    ".mp3",
    ".wav",
    ".flac",
    ".ogg",
    ".wma",
  ],
  fields: [
    { name: "Title", label: "Название", validRegex: "^.{0,50}$" },
    { name: "Album", label: "Альбом", validRegex: "^.{0,50}$" },
    { name: "Genre", label: "Жанр", validRegex: "^.{0,50}$" },
    { name: "Artist", label: "Исполнитель", validRegex: "^.{0,50}$" },
  ],
};

export {
  booksContext,
  documentsContext,
  exerciseMaterialsContext,
  videosContext,
  audioContext,
};
