import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { User } from "./user.entity";
import { RabbitSubscribe } from "@golevelup/nestjs-rabbitmq";
import { MassTransitWrapper } from "../common/events/massTransitWrapper";
import { ProfileUpdated } from "../common/events/profileUpdated";
import { ProfilePhotoUpdated } from "../common/events/profilePhotoUpdated";
import { UserCreated } from "../common/events/userCreated";

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
    let user = await this.usersRepository.findOneBy({ id: msg.message.id });
    user = this.usersRepository.merge(user, msg.message);
    await this.usersRepository.save(user);
  }

  @RabbitSubscribe({
    exchange: "MassTransit.Data.Messages:ProfilePhotoUpdated",
    routingKey: "",
    queue: "profile-photo-updated-certificates",
  })
  public async profilePhotoUpdatedHandler(
    msg: MassTransitWrapper<ProfilePhotoUpdated>
  ) {
    const user = await this.usersRepository.findOneBy({ id: msg.message.id });
    user.avatar = msg.message.avatarHref;
    await this.usersRepository.save(user);
  }

  @RabbitSubscribe({
    exchange: "MassTransit.Data.Messages:UserCreated",
    routingKey: "",
    queue: "user-created-certificates",
  })
  public async userCreatedHandler(msg: MassTransitWrapper<UserCreated>) {
    const user = this.usersRepository.create(msg.message);
    user.avatar = msg.message.avatarHref;
    await this.usersRepository.save(user);
  }
}
