import {
  BadRequestException,
  ForbiddenException,
  Injectable,
  NotFoundException,
  UnauthorizedException,
} from "@nestjs/common";
import { CertificatePreview } from "./dto/response/certificatePreview";
import { CourseService } from "../course/course.service";
import { UserService } from "../user/user.service";
import { OrderRequest } from "./dto/request/orderRequest";
import { OrderIdResponse } from "./dto/response/orderIdResponse";
import { CertificateListCard } from "./dto/response/certificateListCard";
import { OrderService } from "../order/order.service";
import { HttpService } from "@nestjs/axios";
import { ConfigService } from "@nestjs/config";
import { catchError, firstValueFrom } from "rxjs";
import { UserCourseInfoService } from "../user-course-info/user-course-info.service";

@Injectable()
export class CertificateService {
  constructor(
    private userService: UserService,
    private courseService: CourseService,
    private orderService: OrderService,
    private userCourseInfoService: UserCourseInfoService,
    private readonly httpService: HttpService,
    private readonly configService: ConfigService
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
    if (
      !(await this.userCourseInfoService.checkIfUserCanBuyCertificate(
        user,
        course
      ))
    )
      throw new ForbiddenException();
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
    courseId: number,
    orderRequest: OrderRequest
  ): Promise<OrderIdResponse> {
    const user = await this.userService.findOne(userId);
    if (user === null) throw new UnauthorizedException();
    const course = await this.courseService.findOne(courseId);
    if (course === null || !course.hasCertificates)
      throw new NotFoundException();
    if (
      !(await this.userCourseInfoService.checkIfUserCanBuyCertificate(
        user,
        course
      ))
    )
      throw new ForbiddenException();
    orderRequest.address = orderRequest.address?.trim();
    if (orderRequest.address == null || orderRequest.address.length == 0)
      throw new BadRequestException("Address is missing");
    if (orderRequest.address.length > 300)
      throw new BadRequestException("Address too long");
    const response = await firstValueFrom(
      this.httpService
        .get(this.configService.get<string>("MAPBOX_API_URL"), {
          params: {
            q: orderRequest.address,
            country: "ru",
            access_token: this.configService.get<string>("MAPBOX_API_KEY"),
          },
        })
        .pipe(
          catchError((error) => {
            console.error(error);
            throw "An error happened!";
          })
        )
    );
    const features = response.data.features.filter(
      (feature: any) => feature.properties.feature_type === "address"
    );
    if (features.length === 0)
      throw new BadRequestException("Address is not valid");
    if (features[0].properties.full_address != orderRequest.address)
      throw new BadRequestException({
        message: "Address is not valid",
        suggestion: features[0].properties.full_address,
        error: "Bad Request",
        statusCode: 400,
      });
    return {
      orderId: await this.orderService.create(
        user,
        course,
        orderRequest.address
      ),
    } as OrderIdResponse;
  }

  async getAvailableCertificates(userId: string) {
    const user = await this.userService.findOne(userId);
    if (user === null) throw new UnauthorizedException();
    const list =
      await this.userCourseInfoService.getAllAvailableCertificates(user);
    return (
      await Promise.all(
        list.map(async (course) => {
          return {
            predicate: !(await this.orderService.existsOrderWithCourseUser(
              course,
              user
            )),
            course: course,
          };
        })
      )
    )
      .filter((p) => p.predicate)
      .map((p) => p.course)
      .map(
        (course) =>
          <CertificateListCard>{
            courseTitle: course.title,
            courseId: course.id,
          }
      );
  }
}
