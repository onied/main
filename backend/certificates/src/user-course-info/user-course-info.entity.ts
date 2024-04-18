import { Course } from "../course/course.entity";
import { User } from "../user/user.entity";
import { Column, Entity, ManyToOne, PrimaryColumn } from "typeorm";

@Entity()
/**
 * Contains info on user-completed courses.
 * If token === null, user has completed the course,
 * but hadn't bought the certificate yet.
 * If token is a valid jwt token, user has completed the course
 * and bought the certificate.
 */
export class UserCourseInfo {
  @PrimaryColumn()
  @ManyToOne(() => User)
  user: User;

  @PrimaryColumn()
  @ManyToOne(() => Course)
  course: Course;

  @Column()
  token: string | null;
}
