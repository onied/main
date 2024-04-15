import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { Order } from "./order.entity";
import { User } from "../user/user.entity";
import { Course } from "../course/course.entity";

@Injectable()
export class OrderService {
  constructor(
    @InjectRepository(Order)
    private orderRepository: Repository<Order>
  ) {}

  findOne(id: string): Promise<Order | null> {
    return this.orderRepository.findOne({
      where: { id: id },
      relations: {
        user: true,
      },
    });
  }

  async create(user: User, course: Course, address: string): Promise<string> {
    const newOrder = this.orderRepository.create({
      user: user,
      course: course,
      address: address,
    });
    await this.orderRepository.save(newOrder);
    return newOrder.id;
  }
}
