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
import { CertificateListCard } from "./dto/response/certificateListCard";

@Controller("api/v1/certificates")
export class CertificateController {
  constructor(private certificateService: CertificateService) {}
  @Get(":courseId")
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

  @Get()
  async getCertificatesList(
    @Query("userId") userId: string
  ): Promise<CertificateListCard[]> {
    const result =
      await this.certificateService.getAvailableCertificates(userId);
    return result;
  }

  @Post(":courseId/order")
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
