import { useEffect, useRef, useState } from "react";
import api from "../../config/axios";
import { Navigate, useNavigate } from "react-router-dom";
import CustomBeatLoader from "../../components/general/customBeatLoader";

function CreateCourse() {
  const isCreated = useRef(false);
  const navigate = useNavigate();
  const [noAccess, setNoAccess] = useState(false);

  useEffect(() => {
    if (!isCreated.current) {
      isCreated.current = true;
      api
        .post("courses/create")
        .then((response) => {
          navigate("/course/" + response.data.id + "/edit");
        })
        .catch((response) => {
          if (response.response.status == 403) {
            navigate("/subscriptions");
          } else {
            navigate("/login");
          }
        });
    }
  });

  if (noAccess) return <Navigate to="/login"></Navigate>;
  return <CustomBeatLoader />;
}

export default CreateCourse;
