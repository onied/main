import { IQueryHandler, QueryHandler } from "@nestjs/cqrs";
import { GetOrderStatusQuery } from "../queries/get-order-status.query";
import { UserService } from "../../user/user.service";
import { OrderService } from "../order.service";
import { NotFoundException, UnauthorizedException } from "@nestjs/common";

@QueryHandler(GetOrderStatusQuery)
export class GetOrderStatusQueryHandler
  implements IQueryHandler<GetOrderStatusQuery>
{
  constructor(
    private userService: UserService,
    private orderService: OrderService
  ) {}

  async execute(request: GetOrderStatusQuery) {
    const pattern =
      /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
    if (!pattern.test(request.orderId)) throw new NotFoundException();
    const user = await this.userService.findOne(request.userId);
    if (user === null) throw new UnauthorizedException();
    const result = await this.orderService.findOne(request.orderId);
    if (result === null || result.user.id != request.userId)
      throw new NotFoundException();
    return {
      status: result.status,
      dateCreated: result.dateCreated,
      dateUpdated: result.dateUpdated,
    };
  }
}
