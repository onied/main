import api from "../config/axios";
import { Profile } from "../hooks/profile/profile";

class ProfileService {
  static setProfile: ((profile: Profile | null) => void) | undefined =
    undefined;

  static initialize(setProfile: (profile: Profile | null) => void) {
    ProfileService.setProfile = setProfile;
  }

  static fetchProfile() {
    api
      .get("/profile")
      .then((response) =>
        ProfileService.setProfile
          ? ProfileService.setProfile(response.data)
          : undefined
      )
      .catch();
  }
}

export default ProfileService;
