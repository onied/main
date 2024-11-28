import { Controller, Get, Param, Query } from "@nestjs/common";
import { OrderStatus } from "./dto/response/orderStatus";
import { QueryBus } from "@nestjs/cqrs";
import { GetOrderStatusQuery } from "./queries/get-order-status.query";

@Controller("/api/v1/certificates/order/:orderId")
export class OrderController {
  constructor(private queryBus: QueryBus) {}
  @Get()
  async getOrderStatus(
    @Query("userId") userId: string,
    @Param("orderId") orderId: string
  ): Promise<OrderStatus> {
    return await this.queryBus.execute(
      new GetOrderStatusQuery(userId, orderId)
    );
  }
}
