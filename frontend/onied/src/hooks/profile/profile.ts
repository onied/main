export type Profile = {
  firstName: string;
  lastName: string;
  gender: number;
  avatar: string | null;
  email: string;
};

export const getProfileName = (profile: Profile) =>
  profile.firstName + " " + profile.lastName;
