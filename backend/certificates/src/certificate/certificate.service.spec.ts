import { Test, TestingModule } from "@nestjs/testing";
import { CertificateService } from "./certificate.service";
import { UserService } from "../user/user.service";
import { User } from "../user/user.entity";
import { getRepositoryToken } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { CourseService } from "../course/course.service";
import { Course } from "../course/course.entity";
import { OrderService } from "../order/order.service";
import { Order } from "../order/order.entity";
import { HttpService } from "@nestjs/axios";
import { ConfigService } from "@nestjs/config";
import { UserCourseInfoService } from "../user-course-info/user-course-info.service";
import { UserCourseInfo } from "../user-course-info/user-course-info.entity";

describe("CertificateService", () => {
  let service: CertificateService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
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
        UserCourseInfoService,
        {
          provide: getRepositoryToken(UserCourseInfo),
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
      ],
    }).compile();

    service = module.get<CertificateService>(CertificateService);
  });

  it("should be defined", () => {
    expect(service).toBeDefined();
  });
});
