import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { UserCourseInfo } from "./user-course-info.entity";
import { RabbitSubscribe } from "@golevelup/nestjs-rabbitmq";
import {
  PurchaseCreated,
  PurchaseType,
} from "../common/events/purchaseCreated";
import { MassTransitWrapper } from "../common/events/massTransitWrapper";
import { UserService } from "../user/user.service";
import { CourseService } from "../course/course.service";

@Injectable()
export class UserCourseInfoService {
  constructor(
    @InjectRepository(UserCourseInfo)
    private userCourseInfoRepository: Repository<UserCourseInfo>,
    private userService: UserService,
    private courseService: CourseService
  ) {}

  @RabbitSubscribe({
    exchange: "MassTransit.Data.Messages:PurchaseCreated",
    routingKey: "",
    queue: "purchase-created-certificates",
  })
  public async userCreatedHandler(msg: MassTransitWrapper<PurchaseCreated>) {
    if (msg.message.purchaseType !== PurchaseType.Certificate) return;
    const user = await this.userService.findOne(msg.message.userId);
    const course = await this.courseService.findOne(msg.message.courseId);
    await this.userCourseInfoRepository.save({
      user: user,
      course: course,
      token: msg.message.token,
    });
  }
}
