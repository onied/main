import {
  Injectable,
  NotFoundException,
  UnauthorizedException,
} from "@nestjs/common";
import { CertificatePreview } from "./dto/response/certificatePreview";
import { CourseService } from "../course/course.service";
import { UserService } from "../user/user.service";
import { OrderRequest } from "./dto/request/orderRequest";
import { OrderIdResponse } from "./dto/response/orderIdResponse";
import { OrderService } from "src/order/order.service";

@Injectable()
export class CertificateService {
  constructor(
    private userService: UserService,
    private courseService: CourseService,
    private orderService: OrderService
  ) {}

  async getCertificatePreview(
    userId: string,
    courseId: number
  ): Promise<CertificatePreview> {
    const user = await this.userService.findOne(userId);
    if (user === null) throw new UnauthorizedException();
    const course = await this.courseService.findOne(courseId);
    if (course === null || !course.hasCertificates)
      throw new NotFoundException();
    return {
      price: 1000,
      course: {
        title: course.title,
        author: {
          firstName: course.author.firstName,
          lastName: course.author.lastName,
        },
      },
    } as CertificatePreview;
  }

  async createOrder(
    userId: string,
    orderRequest: OrderRequest
  ): Promise<OrderIdResponse> {
    const user = await this.userService.findOne(userId);
    if (user === null) throw new UnauthorizedException();
    const course = await this.courseService.findOne(orderRequest.courseId);
    if (course === null || !course.hasCertificates)
      throw new NotFoundException();
    return {
      orderId: await this.orderService.create(
        user,
        course,
        orderRequest.address
      ),
    } as OrderIdResponse;
  }
}
