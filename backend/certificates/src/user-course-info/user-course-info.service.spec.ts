import { Test, TestingModule } from "@nestjs/testing";
import { UserCourseInfoService } from "./user-course-info.service";
import { getRepositoryToken } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { UserCourseInfo } from "./user-course-info.entity";
import { UserService } from "../user/user.service";
import { CourseService } from "../course/course.service";
import { User } from "../user/user.entity";
import { Course } from "../course/course.entity";
import { HttpService } from "@nestjs/axios";
import { ConfigService } from "@nestjs/config";

describe("UserCourseInfoService", () => {
  let service: UserCourseInfoService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        UserCourseInfoService,
        {
          provide: getRepositoryToken(UserCourseInfo),
          useClass: Repository,
        },
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

    service = module.get<UserCourseInfoService>(UserCourseInfoService);
  });

  it("should be defined", () => {
    expect(service).toBeDefined();
  });
});
