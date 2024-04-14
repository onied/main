import classes from "./manageModerators.module.css";
import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import ModeratorDescription from "./moderatorDescription";
import BeatLoader from "react-spinners/BeatLoader";
import NoAccess from "../../general/responses/noAccess/noAccess";
import NoContent from "../../general/responses/noContent/noContent";

function ManageModerators() {
  const [loadStatus, setLoadStatus] = useState(0);
  const [courseWithStudents, setCourseWithStudents] =
    useState<CourseWithStudents>();

  useEffect(() => {
    setTimeout(() => {
      setLoadStatus(200);
      setCourseWithStudents({ courseId: 0, students: [], title: "" });
      /*setCourseWithStudents(
        {
          courseId: 1,
          title: "Название курса. Как я встретил вашу маму. Осуждаю",
          students: [
            {
              studentId: 1,
              firstName: "Иван",
              lastName: "Иванов",
              isModerator: false
            },
            {
              studentId: 2,
              firstName: "Иван",
              lastName: "Иванов",
              isModerator: false
            },
            {
              studentId: 3,
              firstName: "Иван",
              lastName: "Иванов",
              isModerator: true
            },
            {
              studentId: 4,
              firstName: "Иван",
              lastName: "Иванов",
              isModerator: false
            },
          ]
        }
      );*/
    }, 750);
  }, []);

  switch (loadStatus) {
    case 0:
      return (
        <BeatLoader
          cssOverride={{ margin: "30px 30px" }}
          color="var(--accent-color)"
        ></BeatLoader>
      );
    case 401:
      return <Navigate to="/login"></Navigate>;
    case 403:
      return <NoAccess>У вас нет прав для доступа к этой странице</NoAccess>;
  }

  if (loadStatus !== 200 || courseWithStudents === undefined) return <></>;

  if (courseWithStudents.students.length == 0)
    return (
      <NoContent>
        Нельзя назначить модераторов из-за отсутствия учеников
      </NoContent>
    );

  const appointModerator = (studentId: number) => {
    const newStudents = courseWithStudents.students.map((s) => {
      if (s.studentId === studentId) {
        s.isModerator = true;
      }
      return s;
    });
    setCourseWithStudents({ ...courseWithStudents, students: newStudents });
  };

  const deleteModerator = (studentId: number) => {
    const newStudents = courseWithStudents.students.map((s) => {
      if (s.studentId === studentId) {
        s.isModerator = false;
      }
      return s;
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
  studentId: number;
  firstName: string;
  lastName: string;
  avatarHref?: string;
  isModerator: boolean;
};

export default ManageModerators;
