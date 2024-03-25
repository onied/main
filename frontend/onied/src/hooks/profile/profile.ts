export type Profile = {
  firstName: string;
  lastName: string;
  gender: number;
  avatarHref: string | null;
  email: string;
};

export const getProfileName = (profile: Profile) =>
  profile.firstName + " " + profile.lastName;
