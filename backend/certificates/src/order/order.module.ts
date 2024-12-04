import { Module } from "@nestjs/common";
import { OrderController } from "./order.controller";
import { OrderService } from "./order.service";
import { TypeOrmModule } from "@nestjs/typeorm";
import { Order } from "./order.entity";
import { UserModule } from "../user/user.module";
import { CqrsModule } from "@nestjs/cqrs";
import { GetOrderStatusQueryHandler } from "./handlers/get-order-status.query.handler";

export const QueryHandlers = [GetOrderStatusQueryHandler];

@Module({
  imports: [TypeOrmModule.forFeature([Order]), UserModule, CqrsModule],
  controllers: [OrderController],
  providers: [OrderService, ...QueryHandlers],
  exports: [OrderService],
})
export class OrderModule {}
