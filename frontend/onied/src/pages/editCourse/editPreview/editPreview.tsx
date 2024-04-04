import EditPreviewComponent from "../../../components/editCourse/editPreview/editPreview";
import classes from "../editPreview/editPreview.module.css";
import { useEffect, useState } from "react";
import api from "../../../config/axios";
import { useParams } from "react-router-dom";

function EditPreview() {
  const { courseId } = useParams();
  const [canAccess, setCanAccess] = useState<boolean>(true);
  const noAccess = (
    <h1 style={{ margin: "3rem" }}>У вас нет доступа к этой странице</h1>
  );

  useEffect(() => {
    api
      .put("courses/" + courseId, {
        title: "",
        pictureHref: null,
        description: null,
        hoursCount: null,
        price: null,
        categoryId: null,
        isArchived: null,
        hasCertificates: null,
        isProgramVisible: null,
      })
      .then((response) => console.log(response))
      .catch((error) => {
        console.log(error);
        if (error.response.status === 401) {
          setCanAccess(false);
        }
      });
  }, []);

  if (!canAccess) return noAccess;

  return (
    <div className={classes.container}>
      <EditPreviewComponent />
    </div>
  );
}

export default EditPreview;
