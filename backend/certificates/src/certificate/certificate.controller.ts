import {
  Controller,
  Get,
  Post,
  NotFoundException,
  Param,
  Query,
  Body,
} from "@nestjs/common";
import { CertificatePreview } from "./dto/response/certificatePreview";
import { CertificateService } from "./certificate.service";
import { OrderRequest } from "./dto/request/orderRequest";
import { OrderIdResponse } from "./dto/response/orderIdResponse";

@Controller("api/v1/certificate/:courseId")
export class CertificateController {
  constructor(private certificateService: CertificateService) {}
  @Get()
  async getCertificatePreview(
    @Query("userId") userId: string,
    @Param("courseId") courseId: string
  ): Promise<CertificatePreview> {
    console.log("here");
    const nCourseId = Number(courseId);
    if (isNaN(nCourseId)) throw new NotFoundException();
    const result = await this.certificateService.getCertificatePreview(
      userId,
      nCourseId
    );
    return result;
  }

  @Post("order")
  createOrder(
    @Query("userId") userId: string,
    @Param("courseId") courseId: string,
    @Body() orderRequest: OrderRequest
  ): Promise<OrderIdResponse> {
    const nCourseId = Number(courseId);
    if (isNaN(nCourseId)) throw new NotFoundException();
    return this.certificateService.createOrder(userId, nCourseId, orderRequest);
  }
}
