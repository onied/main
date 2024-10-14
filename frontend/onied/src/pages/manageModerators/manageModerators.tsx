import classes from "./manageModerators.module.css";
import { useEffect, useState } from "react";
import { Navigate, useParams } from "react-router-dom";
import ModeratorDescription from "./moderatorDescription";
import NoAccess from "../../components/general/responses/noAccess/noAccess";
import NoContent from "../../components/general/responses/noContent/noContent";
import NotFound from "../../components/general/responses/notFound/notFound";
import api from "../../config/axios";
import CustomBeatLoader from "../../components/general/customBeatLoader";

function ManageModerators() {
  const { courseId } = useParams();

  const [loadStatus, setLoadStatus] = useState(0);
  const [courseWithStudents, setCourseWithStudents] =
    useState<CourseWithStudents>();
  const id = Number(courseId);

  useEffect(() => {
    if (isNaN(id)) setLoadStatus(404);

    api
      .get("courses/" + courseId + "/moderators")
      .then((response) => {
        console.log(response.data);
        setCourseWithStudents(response.data);
        setLoadStatus(200);
      })
      .catch((error) => {
        if (error.response?.status) {
          switch (error.response?.status) {
            case 401:
              setLoadStatus(401);
              break;
            case 403:
              setLoadStatus(403);
              break;
            case 404:
              setLoadStatus(404);
              break;
          }
        }
      });
  }, []);

  switch (loadStatus) {
    case 0:
      return <CustomBeatLoader />;
    case 401:
      return <Navigate to="/login"></Navigate>;
    case 403:
      return <NoAccess>У вас нет прав для доступа к этой странице</NoAccess>;
    case 404:
      return <NotFound>Курс не найден</NotFound>;
  }

  if (loadStatus !== 200 || courseWithStudents === undefined) return <></>;

  if (courseWithStudents.students.length == 0)
    return (
      <NoContent>
        Нельзя назначить модераторов из-за отсутствия учеников
      </NoContent>
    );

  const appointModerator = (studentId: string) => {
    const newStudents = courseWithStudents.students.map((s) => {
      if (s.studentId === studentId) {
        s.isModerator = true;
      }
      return s;
    });
    api
      .patch("courses/" + id + "/moderators/add", { studentId: studentId })
      .catch((error) => {
        if (error.response?.status) {
          switch (error.response?.status) {
            case 401:
              setLoadStatus(401);
              break;
            case 403:
              setLoadStatus(403);
              break;
            case 404:
              setLoadStatus(404);
              break;
          }
        }
      });
    setCourseWithStudents({ ...courseWithStudents, students: newStudents });
  };

  const deleteModerator = (studentId: string) => {
    const newStudents = courseWithStudents.students.map((s) => {
      if (s.studentId === studentId) {
        s.isModerator = false;
      }
      return s;
    });
    api
      .patch("courses/" + id + "/moderators/delete", { studentId: studentId })
      .catch((error) => {
        if (error.response?.status) {
          switch (error.response?.status) {
            case 401:
              setLoadStatus(401);
              break;
            case 403:
              setLoadStatus(403);
              break;
            case 404:
              setLoadStatus(404);
              break;
          }
        }
      });
    setCourseWithStudents({ ...courseWithStudents, students: newStudents });
  };

  return (
    <div className={classes.container}>
      <h2 className={classes.title}>Назначение модераторов</h2>
      <p className={classes.titleCourse}>{courseWithStudents.title}</p>
      {courseWithStudents.students.map((student) => (
        <div key={student.studentId} className={classes.card}>
          <ModeratorDescription {...student}></ModeratorDescription>
          {!student.isModerator ? (
            <button
              onClick={() => appointModerator(student.studentId)}
              className={classes.appoint}
            >
              назначить модератором
            </button>
          ) : (
            <button
              onClick={() => deleteModerator(student.studentId)}
              className={classes.delete}
            >
              удалить модератора
            </button>
          )}
        </div>
      ))}
    </div>
  );
}

type CourseWithStudents = {
  courseId: number;
  title: string;
  students: Array<Student>;
};

export type Student = {
  studentId: string;
  firstName: string;
  lastName: string;
  avatarHref?: string;
  isModerator: boolean;
};

export default ManageModerators;
