import { BeatLoader } from "react-spinners";

export default function CustomBeatLoader() {
  return (
    <BeatLoader
      color="var(--accent-color)"
      style={{
        margin: "3rem",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    />
  );
}
