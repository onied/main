import { Test, TestingModule } from "@nestjs/testing";
import { OrderController } from "./order.controller";
import { OrderService } from "./order.service";
import { Repository } from "typeorm";
import { Order, Status } from "./order.entity";
import { getRepositoryToken } from "@nestjs/typeorm";
import { User } from "../user/user.entity";
import { UserService } from "../user/user.service";
import { NotFoundException, UnauthorizedException } from "@nestjs/common";
import { Course } from "../course/course.entity";
import { OrderStatus } from "./dto/response/orderStatus";
import { CqrsModule } from "@nestjs/cqrs";
import { QueryHandlers } from "./order.module";

describe("OrderController", () => {
  let controller: OrderController;
  let orderService: OrderService;
  let userService: UserService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      imports: [CqrsModule],
      controllers: [OrderController],
      providers: [
        {
          provide: OrderService,
          useClass: OrderService,
        },
        {
          provide: getRepositoryToken(Order),
          useClass: Repository,
        },
        {
          provide: UserService,
          useClass: UserService,
        },
        {
          provide: getRepositoryToken(User),
          useClass: Repository,
        },
        ...QueryHandlers,
      ],
    }).compile();

    await module.init();

    controller = module.get<OrderController>(OrderController);
    userService = module.get<UserService>(UserService);
    orderService = module.get<OrderService>(OrderService);
  });

  it("should be defined", () => {
    expect(controller).toBeDefined();
  });

  it("should return notfound if orderId is incorrect", async () => {
    // Arrange

    // Act

    const promise = controller.getOrderStatus(
      "a31c13c6-09a3-44e1-a6dd-98b747f992f6",
      "asjldkfjskdfj"
    );

    // Assert

    await expect(promise).rejects.toThrow(NotFoundException);
  });

  it("should return unauthorized if userId is incorrect", async () => {
    // Arrange

    jest
      .spyOn(userService, "findOne")
      .mockResolvedValueOnce(Promise.resolve(null));

    // Act

    const promise = controller.getOrderStatus(
      "asdf",
      "a31c13c6-09a3-44e1-a6dd-98b747f992f6"
    );

    // Assert

    await expect(promise).rejects.toThrow(UnauthorizedException);
  });

  it("should return not found if order is not found", async () => {
    // Arrange

    const author: User = {
      id: "70cd45d8-4dd1-4576-8f63-10afcfaf9b46",
      firstName: "asdf",
      lastName: "lkj",
      avatar: "",
      gender: 0,
    };

    const user: User = {
      id: "3c839d46-e349-4a50-8429-ec7b9298be8d",
      firstName: "sdfsdf",
      lastName: "fdsfds",
      avatar: "",
      gender: 0,
    };

    const course: Course = {
      id: 1,
      title: "asdfasdf",
      author: author,
      hasCertificates: true,
    };

    const order: Order = {
      id: "066c23b2-cabf-4a7f-a6cd-e2c8b9e92425",
      course: course,
      user: user,
      address: "asdfasdf",
      dateCreated: new Date(),
      dateUpdated: new Date(),
      status: Status.CREATED,
    };

    jest
      .spyOn(userService, "findOne")
      .mockResolvedValueOnce(Promise.resolve(user));
    jest
      .spyOn(orderService, "findOne")
      .mockResolvedValueOnce(Promise.resolve(null));

    // Act

    const promise = controller.getOrderStatus(user.id, order.id);

    // Assert

    await expect(promise).rejects.toThrow(NotFoundException);
  });

  it("should return order if order is found", async () => {
    // Arrange

    const author: User = {
      id: "70cd45d8-4dd1-4576-8f63-10afcfaf9b46",
      firstName: "asdf",
      lastName: "lkj",
      avatar: "",
      gender: 0,
    };

    const user: User = {
      id: "3c839d46-e349-4a50-8429-ec7b9298be8d",
      firstName: "sdfsdf",
      lastName: "fdsfds",
      avatar: "",
      gender: 0,
    };

    const course: Course = {
      id: 1,
      title: "asdfasdf",
      author: author,
      hasCertificates: true,
    };

    const order: Order = {
      id: "066c23b2-cabf-4a7f-a6cd-e2c8b9e92425",
      course: course,
      user: user,
      address: "asdfasdf",
      dateCreated: new Date(),
      dateUpdated: new Date(),
      status: Status.CREATED,
    };

    jest
      .spyOn(userService, "findOne")
      .mockResolvedValueOnce(Promise.resolve(user));
    jest
      .spyOn(orderService, "findOne")
      .mockResolvedValueOnce(Promise.resolve(order));

    // Act

    const promise = controller.getOrderStatus(user.id, order.id);

    // Assert

    await expect(promise).resolves.toStrictEqual(<OrderStatus>{
      status: order.status,
      dateCreated: order.dateCreated,
      dateUpdated: order.dateUpdated,
    });
  });
});
