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
import { OrderRequest } from "./dto/request/orderRequest";
import { OrderIdResponse } from "./dto/response/orderIdResponse";
import { CertificateListCard } from "./dto/response/certificateListCard";
import { CommandBus, QueryBus } from "@nestjs/cqrs";
import { GetCertificatePreviewQuery } from "./queries/get-certificate-preview.query";
import { GetCertificatesListQuery } from "./queries/get-certificates-list.query";
import { CreateOrderCommand } from "./commands/create-order.command";

@Controller("api/v1/certificates")
export class CertificateController {
  constructor(
    private commandBus: CommandBus,
    private queryBus: QueryBus
  ) {}
  @Get(":courseId")
  async getCertificatePreview(
    @Query("userId") userId: string,
    @Param("courseId") courseId: string
  ): Promise<CertificatePreview> {
    const nCourseId = Number(courseId);
    if (isNaN(nCourseId)) throw new NotFoundException();
    const result = await this.queryBus.execute(
      new GetCertificatePreviewQuery(userId, nCourseId)
    );
    return result;
  }

  @Get()
  async getCertificatesList(
    @Query("userId") userId: string
  ): Promise<CertificateListCard[]> {
    const result = await this.queryBus.execute(
      new GetCertificatesListQuery(userId)
    );
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
    return this.commandBus.execute(
      new CreateOrderCommand(userId, nCourseId, orderRequest)
    );
  }
}
