export class GetOrderStatusQuery {
  constructor(
    public readonly userId: string,
    public readonly orderId: string
  ) {}
}
