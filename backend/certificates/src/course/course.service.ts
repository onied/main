import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { Course } from "./course.entity";
import { CourseCreated } from "../common/events/courseCreated";
import { MassTransitWrapper } from "../common/events/massTransitWrapper";
import { RabbitSubscribe } from "@golevelup/nestjs-rabbitmq";
import { CourseUpdated } from "../common/events/courseUpdated";
import { User } from "../user/user.entity";

@Injectable()
export class CourseService {
  constructor(
    @InjectRepository(Course)
    private courseRepository: Repository<Course>,
    @InjectRepository(User)
    private userRepository: Repository<User>
  ) {}

  findOne(id: number): Promise<Course | null> {
    return this.courseRepository.findOne({
      where: { id: id },
      relations: {
        author: true,
      },
    });
  }

  @RabbitSubscribe({
    exchange: "MassTransit.Data.Messages:CourseCreated",
    routingKey: "",
    queue: "course-created-certificates",
  })
  public async courseCreatedHandler(msg: MassTransitWrapper<CourseCreated>) {
    const course = this.courseRepository.create(msg.message);
    const author = await this.userRepository.findOneBy({
      id: msg.message.authorId,
    });
    course.author = author;
    await this.courseRepository.save(course);
  }

  @RabbitSubscribe({
    exchange: "MassTransit.Data.Messages:CourseUpdated",
    routingKey: "",
    queue: "course-updated-certificates",
  })
  public async courseUpdatedHandler(msg: MassTransitWrapper<CourseUpdated>) {
    let course = await this.courseRepository.findOneBy({
      id: msg.message.id,
    });
    course = this.courseRepository.merge(course, msg.message);
    await this.courseRepository.save(course);
  }
}
