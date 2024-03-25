import ProfileSidebar from "../../components/sidebar/profileSidebar";
import { useProfile } from "../../hooks/profile/useProfile";
import { Navigate } from "react-router-dom";

function ProfilePage() {
  const profile = useProfile();
  if (profile == null) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <ProfileSidebar></ProfileSidebar>
    </>
  );
}

export default ProfilePage;
