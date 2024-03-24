export type Profile = {
  firstName: string;
  lastName: string;
  gender: number;
  avatarHref: string;
  email: string;
};

export const getProfileName = (profile: Profile) =>
  profile.firstName + " " + profile.lastName;
