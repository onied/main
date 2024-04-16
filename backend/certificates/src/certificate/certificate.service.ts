import {
  BadRequestException,
  Injectable,
  NotFoundException,
  UnauthorizedException,
} from "@nestjs/common";
import { CertificatePreview } from "./dto/response/certificatePreview";
import { CourseService } from "../course/course.service";
import { UserService } from "../user/user.service";
import { OrderRequest } from "./dto/request/orderRequest";
import { OrderIdResponse } from "./dto/response/orderIdResponse";
import { OrderService } from "../order/order.service";
import { HttpService } from "@nestjs/axios";
import { ConfigService } from "@nestjs/config";
import { catchError, firstValueFrom } from "rxjs";

@Injectable()
export class CertificateService {
  constructor(
    private userService: UserService,
    private courseService: CourseService,
    private orderService: OrderService,
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
    orderRequest.address = orderRequest.address?.trim();
    if (orderRequest.address == null || orderRequest.address.length == 0)
      throw new BadRequestException("Address is missing");
    if (orderRequest.address.length > 300)
      throw new BadRequestException("Address too long");
    const response = await firstValueFrom(
      this.httpService
        .get("https://api.mapbox.com/search/geocode/v6/forward", {
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
}
