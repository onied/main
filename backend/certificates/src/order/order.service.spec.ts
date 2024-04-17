import { Test, TestingModule } from "@nestjs/testing";
import { OrderService } from "./order.service";
import { Repository } from "typeorm";
import { Order, Status } from "./order.entity";
import { getRepositoryToken } from "@nestjs/typeorm";
import { Course } from "../course/course.entity";
import { User } from "../user/user.entity";

describe("OrderService", () => {
  let service: OrderService;
  let repo: Repository<Order>;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        OrderService,
        {
          provide: getRepositoryToken(Order),
          useClass: Repository,
        },
      ],
    }).compile();

    service = module.get<OrderService>(OrderService);
    repo = module.get<Repository<Order>>(getRepositoryToken(Order));
  });

  it("should be defined", () => {
    expect(service).toBeDefined();
  });

  it("should return for findOne", async () => {
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

    const id = "066c23b2-cabf-4a7f-a6cd-e2c8b9e92425";

    const order: Order = {
      id: id,
      course: course,
      user: user,
      address: "asdfasdf",
      dateCreated: new Date(),
      dateUpdated: new Date(),
      status: Status.CREATED,
    };
    jest.spyOn(repo, "findOne").mockResolvedValueOnce(order);

    // Act

    const result = await service.findOne(id);

    // Assert

    expect(result).toEqual(order);
  });

  it("should return order id when creating", async () => {
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
    jest.spyOn(repo, "create").mockReturnValueOnce(order);
    jest.spyOn(repo, "save").mockReturnValueOnce(undefined);

    // Act

    const result = await service.create(user, course, order.address);

    // Assert

    expect(result).toEqual(order.id);
  });
});
