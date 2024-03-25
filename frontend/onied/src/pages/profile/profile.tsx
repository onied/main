import ProfilePageContainer from "../../components/profile/profilePageContainer";
import ProfileSidebar from "../../components/sidebar/profileSidebar";
import { useProfile } from "../../hooks/profile/useProfile";
import { Navigate } from "react-router-dom";

function ProfilePage() {
  const profile = useProfile();
  if (profile == null) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <ProfileSidebar></ProfileSidebar>
      <ProfilePageContainer>I like trains</ProfilePageContainer>
    </>
  );
}

export default ProfilePage;
