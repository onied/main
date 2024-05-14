import { Course } from "../course/course.entity";
import { User } from "../user/user.entity";
import {
  Column,
  CreateDateColumn,
  Entity,
  ManyToOne,
  PrimaryGeneratedColumn,
  UpdateDateColumn,
} from "typeorm";

export enum Status {
  CREATED = "Created",
  PRINTING = "Printing",
  SENDING = "Sending",
  IN_DELIVERY = "InDelivery",
  DELIVERED = "Delivered",
}

@Entity()
export class Order {
  @PrimaryGeneratedColumn("uuid")
  id: string;

  @ManyToOne(() => Course)
  course: Course;

  @ManyToOne(() => User)
  user: User;

  @Column({ length: 300 })
  address: string;

  @CreateDateColumn()
  dateCreated: Date;

  @UpdateDateColumn()
  dateUpdated: Date;

  @Column({
    type: "enum",
    enum: Status,
    default: Status.CREATED,
  })
  status: Status;
}
