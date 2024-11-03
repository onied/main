import { useEffect, useRef } from "react";
import api from "../../config/axios";
import { useNavigate } from "react-router-dom";
import CustomBeatLoader from "../../components/general/customBeatLoader";

function CreateCourse() {
  const isCreated = useRef(false);
  const navigate = useNavigate();

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

  return <CustomBeatLoader />;
}

export default CreateCourse;
