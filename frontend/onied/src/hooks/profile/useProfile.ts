import { useContext } from "react";
import { ProfileContext } from "./profileContext";
import { LoadingContext } from "./loadingContext";
import { Profile } from "./profile";

export const useProfile: () => [Profile | null, boolean] = () => [
  useContext(ProfileContext),
  useContext(LoadingContext),
];
