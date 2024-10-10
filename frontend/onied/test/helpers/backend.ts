import Config from "../../src/config/config";

const backend = (path: string) => {
  return Config.BaseURL.replace(/\/$/, "") + "/" + path.replace(/^\//, "");
};

export default backend;
