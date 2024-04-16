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
    const nCourseId = Number(courseId);
    if (isNaN(nCourseId)) throw new NotFoundException();
    const result = await this.certificateService.getCertificatePreview(
      userId,
      nCourseId
    );
    return result;
  }

  @Post()
  createOrder(
    @Query("userId") userId: string,
    @Body() orderRequest: OrderRequest
  ): Promise<OrderIdResponse> {
    return this.certificateService.createOrder(userId, orderRequest);
  }
}
