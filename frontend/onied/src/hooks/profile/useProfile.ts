import { useContext } from "react";
import { ProfileContext } from "./profileContext";

export const useProfile = () => useContext(ProfileContext);
