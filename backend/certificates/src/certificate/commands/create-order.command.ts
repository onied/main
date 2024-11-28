import { OrderRequest } from "../dto/request/orderRequest";

export class CreateOrderCommand {
  constructor(
    public readonly userId: string,
    public readonly courseId: number,
    public readonly orderRequest: OrderRequest
  ) {}
}
