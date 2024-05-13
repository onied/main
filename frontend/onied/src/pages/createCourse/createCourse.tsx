import { useEffect, useRef, useState } from "react";
import api from "../../config/axios";
import { Navigate, useNavigate } from "react-router-dom";
import { BeatLoader } from "react-spinners";

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
  return <BeatLoader color="var(--accent-color)"></BeatLoader>;
}

export default CreateCourse;
