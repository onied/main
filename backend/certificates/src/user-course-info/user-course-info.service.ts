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
import { CourseService } from "../course/course.service";
import { User } from "../user/user.entity";
import { Course } from "../course/course.entity";
import { PurchasesServiceClient } from "../grpc-generated/purchases.client";
import {
  VerificationOutcome,
  VerifyTokenRequest,
} from "../grpc-generated/purchases";

@Injectable()
export class UserCourseInfoService {
  constructor(
    @InjectRepository(UserCourseInfo)
    private userCourseInfoRepository: Repository<UserCourseInfo>,
    private courseService: CourseService,
    private readonly purchasesServiceClient: PurchasesServiceClient
  ) {}

  public async checkIfUserCanBuyCertificate(
    user: User,
    course: Course
  ): Promise<boolean> {
    const completed = await this.userCourseInfoRepository.findOneBy({
      userId: user.id,
      courseId: course.id,
    });
    if (completed === null) return false; // User has not completed the course yet

    const request: VerifyTokenRequest = { token: completed.token };
    const reply = await this.purchasesServiceClient.verify(request);

    return reply.response.verificationOutcome === VerificationOutcome.OK;
  }

  public async getAllAvailableCertificates(user: User) {
    const res = (
      await this.userCourseInfoRepository.findBy({ userId: user.id })
    ).map((uci) => uci.courseId);
    return await this.courseService.findInList(res);
  }

  @RabbitSubscribe({
    exchange: "MassTransit.Data.Messages:PurchaseCreated",
    routingKey: "",
    queue: "purchase-created-certificates",
  })
  public async userCreatedHandler(msg: MassTransitWrapper<PurchaseCreated>) {
    if (msg.message.purchaseType !== PurchaseType.Course) return;
    await this.userCourseInfoRepository.save(msg.message as UserCourseInfo);
  }
}
