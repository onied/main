import api from "../config/axios";
import { Profile } from "../hooks/profile/profile";

class ProfileService {
  static setProfile: ((profile: Profile | null) => void) | undefined =
    undefined;
  static setLoading: ((loading: boolean) => void) | undefined = undefined;

  static initialize(
    setProfile: (profile: Profile | null) => void,
    setLoading: (loading: boolean) => void
  ) {
    ProfileService.setProfile = setProfile;
    ProfileService.setLoading = setLoading;
  }

  static fetchProfile() {
    if (
      ProfileService.setProfile === undefined ||
      ProfileService.setLoading === undefined
    )
      return;
    ProfileService.setLoading(true);
    api
      .get("/profile")
      .then((response) => ProfileService.setProfile!(response.data))
      .catch()
      .finally(() => ProfileService.setLoading!(false));
  }
}

export default ProfileService;
