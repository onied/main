import { CommandHandler, ICommandHandler } from "@nestjs/cqrs";
import { CreateOrderCommand } from "../commands/create-order.command";
import { CertificateService } from "../certificate.service";

@CommandHandler(CreateOrderCommand)
export class CreateOrderCommandHandler
  implements ICommandHandler<CreateOrderCommand>
{
  constructor(private service: CertificateService) {}

  async execute(command: CreateOrderCommand) {
    return await this.service.createOrder(
      command.userId,
      command.courseId,
      command.orderRequest
    );
  }
}
