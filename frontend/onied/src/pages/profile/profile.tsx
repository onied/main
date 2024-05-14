import ProfileCertificates from "../../components/profile/certificates";
import ProfileCourses from "../../components/profile/courses";
import ProfileInfo from "../../components/profile/info";
import ProfilePageContainer from "../../components/profile/profilePageContainer";
import ProfileSidebar from "../../components/sidebar/profileSidebar";
import { useProfile } from "../../hooks/profile/useProfile";
import { Navigate, Route, Routes } from "react-router-dom";
import ProfilePurchases from "../../components/profile/purchases";
import ProfileSubcriptions from "../../components/profile/subscriptions";

function ProfilePage() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <ProfileSidebar></ProfileSidebar>
      <ProfilePageContainer>
        <Routes>
          <Route path="/" element={<ProfileInfo />} />
          <Route path="/courses" element={<ProfileCourses />} />
          <Route path="/certificates" element={<ProfileCertificates />} />
          <Route path="/purchases" element={<ProfilePurchases />} />
          <Route path="/subscriptions" element={<ProfileSubcriptions />} />
          <Route path="*" element={<Navigate to="/profile" />} />
        </Routes>
      </ProfilePageContainer>
    </>
  );
}

export default ProfilePage;
