import { Test, TestingModule } from "@nestjs/testing";
import { CertificateController } from "./certificate.controller";
import { CertificateService } from "./certificate.service";
import { UserService } from "../user/user.service";
import { getRepositoryToken } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { User } from "../user/user.entity";
import { CourseService } from "../course/course.service";
import { Course } from "../course/course.entity";
import { OrderService } from "../order/order.service";
import { Order } from "../order/order.entity";
import { HttpService } from "@nestjs/axios";
import { ConfigService } from "@nestjs/config";

describe("CertificateController", () => {
  let controller: CertificateController;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      controllers: [CertificateController],
      providers: [
        CertificateService,
        UserService,
        {
          provide: getRepositoryToken(User),
          useClass: Repository,
        },
        CourseService,
        {
          provide: getRepositoryToken(Course),
          useClass: Repository,
        },
        OrderService,
        {
          provide: getRepositoryToken(Order),
          useClass: Repository,
        },
        {
          provide: HttpService,
          useValue: {
            get: jest.fn(),
          },
        },
        {
          provide: ConfigService,
          useValue: {
            get: () => "",
          },
        },
        CertificateService,
      ],
    }).compile();

    controller = module.get<CertificateController>(CertificateController);
  });

  it("should be defined", () => {
    expect(controller).toBeDefined();
  });
});
