import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { User } from "./user.entity";
import { RabbitSubscribe } from "@golevelup/nestjs-rabbitmq";
import { MassTransitWrapper } from "../common/events/massTransitWrapper";
import { ProfileUpdated } from "../common/events/profileUpdated";

@Injectable()
export class UserService {
  constructor(
    @InjectRepository(User)
    private usersRepository: Repository<User>
  ) {}

  findOne(id: string): Promise<User | null> {
    return this.usersRepository.findOneBy({ id });
  }

  @RabbitSubscribe({
    exchange: "MassTransit.Data.Messages:ProfileUpdated",
    routingKey: "",
    queue: "profile-updated-certificates",
  })
  public async profileUpdatedHandler(msg: MassTransitWrapper<ProfileUpdated>) {
    const user = await this.usersRepository.findOneBy({ id: msg.message.id });
    user.firstName = msg.message.firstName;
    user.lastName = msg.message.lastName;
    user.gender = msg.message.gender;
    this.usersRepository.merge(user, msg.message);
    await this.usersRepository.save(user);
  }
}
