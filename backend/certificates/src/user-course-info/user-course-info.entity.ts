import { Column, Entity, PrimaryColumn } from "typeorm";

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
  userId: string;

  @PrimaryColumn()
  courseId: number;

  @Column()
  token: string | null;
}
