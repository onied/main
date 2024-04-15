import { User } from "src/user/user.entity";
import { Entity, Column, PrimaryColumn, ManyToOne } from "typeorm";

@Entity()
export class Course {
  @PrimaryColumn()
  id: number;

  @Column()
  title: string;

  @ManyToOne(() => User)
  author: User;

  @Column()
  hasCertificates: boolean;
}
