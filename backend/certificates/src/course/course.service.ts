import { Inject, Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { In, Repository } from "typeorm";
import { Course } from "./course.entity";
import { CourseCreated } from "../common/events/courseCreated";
import { MassTransitWrapper } from "../common/events/massTransitWrapper";
import { RabbitSubscribe, AmqpConnection } from "@golevelup/nestjs-rabbitmq";
import { CourseUpdated } from "../common/events/courseUpdated";
import { User } from "../user/user.entity";
import { CourseCreateFailed } from "../common/events/courseCreateFailed";

@Injectable()
export class CourseService {
  constructor(
    @InjectRepository(Course)
    private courseRepository: Repository<Course>,
    @InjectRepository(User)
    private userRepository: Repository<User>,
    @Inject(AmqpConnection)
    private readonly amqpConnection: AmqpConnection
  ) {}

  findOne(id: number): Promise<Course | null> {
    return this.courseRepository.findOne({
      where: { id: id },
      relations: {
        author: true,
      },
    });
  }

  findInList(ids: number[]): Promise<Course[]> {
    return this.courseRepository.find({
      where: { id: In(ids) },
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
    try {
      const course = this.courseRepository.create(msg.message);
      const author = await this.userRepository.findOneBy({
        id: msg.message.authorId,
      });
      course.author = author;
      await this.courseRepository.save(course);
    } catch (error) {
      const event = {
        messageType: [
          "urn:message:MassTransit.Data.Messages:CourseCreateFailed",
        ],
        message: {
          id: msg.message.id,
          errorMessage: error.message,
        },
      };
      await this.amqpConnection.publish(
        "course-create-failed-courses",
        "",
        event
      );
      await this.amqpConnection.publish(
        "course-create-failed-purchases",
        "",
        event
      );
    }
  }

  @RabbitSubscribe({
    exchange: "MassTransit.Data.Messages:CourseCreateFailed",
    routingKey: "",
    queue: "course-create-failed-certificates",
  })
  public async courseCreateFailedHandler(
    msg: MassTransitWrapper<CourseCreateFailed>
  ) {
    const course = await this.courseRepository.findOneBy({
      id: msg.message.id,
    });
    if (course === null) return;
    await this.courseRepository.delete(course);
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
    if (course === null) return;
    course = this.courseRepository.merge(course, msg.message);
    await this.courseRepository.save(course);
  }
}
