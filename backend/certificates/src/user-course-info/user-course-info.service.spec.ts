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
import { of, throwError } from "rxjs";
import { ForbiddenException } from "@nestjs/common";
import { AmqpConnection } from "@golevelup/nestjs-rabbitmq";

describe("UserCourseInfoService", () => {
  let service: UserCourseInfoService;
  let courseService: CourseService;
  let repo: Repository<UserCourseInfo>;
  let httpService: HttpService;
  let amqpConnection: AmqpConnection;

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
            post: () =>
              of({
                data: {},
                headers: {},
                status: 200,
                statusText: "OK",
              }),
          },
        },
        AmqpConnection,
        {
          provide: ConfigService,
          useValue: {
            get: () => "",
          },
        },
      ],
    }).compile();

    service = module.get<UserCourseInfoService>(UserCourseInfoService);
    courseService = module.get<CourseService>(CourseService);
    repo = module.get<Repository<UserCourseInfo>>(
      getRepositoryToken(UserCourseInfo)
    );
    httpService = module.get<HttpService>(HttpService);
    amqpConnection = module.get<AmqpConnection>(AmqpConnection);
  });

  it("should be defined", () => {
    expect(service).toBeDefined();
  });

  it("should return all available certificates", async () => {
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

    const courses = [
      {
        id: 1,
        title: "asdf",
        author: author,
        hasCertificates: true,
      },
      {
        id: 2,
        title: "asdf",
        author: author,
        hasCertificates: true,
      },
      {
        id: 3,
        title: "asdf",
        author: author,
        hasCertificates: true,
      },
    ];

    jest.spyOn(repo, "findBy").mockReturnValueOnce(
      Promise.resolve([
        {
          userId: user.id,
          courseId: 1,
          token: "",
        },
        {
          userId: user.id,
          courseId: 2,
          token: "",
        },
        {
          userId: user.id,
          courseId: 3,
          token: "",
        },
      ])
    );

    jest
      .spyOn(courseService, "findInList")
      .mockReturnValueOnce(Promise.resolve(courses));

    // Act

    const result = await service.getAllAvailableCertificates(user);

    // Assert

    expect(result).toEqual(courses);
  });

  it("should allow user to buy certificate if they completed the course and token is fine", async () => {
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

    const course = {
      id: 1,
      title: "asdf",
      author: author,
      hasCertificates: true,
    };

    jest.spyOn(repo, "findOneBy").mockReturnValueOnce(
      Promise.resolve({
        userId: user.id,
        courseId: 1,
        token: "",
      })
    );

    // Act

    const result = await service.checkIfUserCanBuyCertificate(user, course);

    // Assert

    expect(result).toEqual(true);
  });

  it("should deny user to buy certificate if they didn't complete the course", async () => {
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

    const course = {
      id: 1,
      title: "asdf",
      author: author,
      hasCertificates: true,
    };

    jest.spyOn(repo, "findOneBy").mockReturnValueOnce(Promise.resolve(null));

    // Act

    const result = await service.checkIfUserCanBuyCertificate(user, course);

    // Assert

    expect(result).toEqual(false);
  });

  it("should deny user to buy certificate if the token is expired", async () => {
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

    const course = {
      id: 1,
      title: "asdf",
      author: author,
      hasCertificates: true,
    };

    jest.spyOn(repo, "findOneBy").mockReturnValueOnce(
      Promise.resolve({
        userId: user.id,
        courseId: 1,
        token: "",
      })
    );
    jest.spyOn(httpService, "post").mockReturnValueOnce(throwError(() => {}));

    // Act

    try {
      const result = await service.checkIfUserCanBuyCertificate(user, course);
      expect(result).toEqual(false);
    } catch (error: unknown) {
      expect(error).toBeInstanceOf(ForbiddenException);
    }

    // Assert
  });
});
