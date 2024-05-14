import {
  Controller,
  Get,
  NotFoundException,
  Param,
  Query,
  UnauthorizedException,
} from "@nestjs/common";
import { OrderService } from "./order.service";
import { UserService } from "../user/user.service";
import { OrderStatus } from "./dto/response/orderStatus";

@Controller("/api/v1/certificates/order/:orderId")
export class OrderController {
  constructor(
    private orderService: OrderService,
    private userService: UserService
  ) {}
  @Get()
  async getOrderStatus(
    @Query("userId") userId: string,
    @Param("orderId") orderId: string
  ): Promise<OrderStatus> {
    const pattern =
      /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
    if (!pattern.test(orderId)) throw new NotFoundException();
    const user = await this.userService.findOne(userId);
    if (user === null) throw new UnauthorizedException();
    const result = await this.orderService.findOne(orderId);
    if (result === null || result.user.id != userId)
      throw new NotFoundException();
    return {
      status: result.status,
      dateCreated: result.dateCreated,
      dateUpdated: result.dateUpdated,
    };
  }
}
