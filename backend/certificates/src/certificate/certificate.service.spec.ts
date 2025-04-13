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
import {
  BadRequestException,
  ForbiddenException,
  NotFoundException,
  UnauthorizedException,
} from "@nestjs/common";
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";
import { OrderRequest } from "./dto/request/orderRequest";
import { OrderIdResponse } from "./dto/response/orderIdResponse";
import { VerificationOutcome } from "../grpc-generated/purchases";
import { PurchasesServiceClient } from "../grpc-generated/purchases.client";
import { AmqpConnection } from "@golevelup/nestjs-rabbitmq";

describe("CertificateService", () => {
  let service: CertificateService;
  let userService: UserService;
  let courseService: CourseService;
  let userCourseInfoService: UserCourseInfoService;
  let httpService: HttpService;
  let orderService: OrderService;

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
          provide: PurchasesServiceClient,
          useValue: {
            verify: jest.fn().mockResolvedValue({
              verificationOutcome: VerificationOutcome.OK,
            }),
          },
        },
        {
          provide: ConfigService,
          useValue: {
            get: () => "",
          },
        },
        {
          provide: AmqpConnection,
          useValue: {
            publish: jest.fn(),
          },
        },
      ],
    }).compile();

    service = module.get<CertificateService>(CertificateService);
    userService = module.get<UserService>(UserService);
    courseService = module.get<CourseService>(CourseService);
    userCourseInfoService = module.get<UserCourseInfoService>(
      UserCourseInfoService
    );
    httpService = module.get<HttpService>(HttpService);
    orderService = module.get<OrderService>(OrderService);
  });

  it("should be defined", () => {
    expect(service).toBeDefined();
  });

  describe("getCertificatePreview", () => {
    it("should throw Unauthorized when user does not exist", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(null);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);

      // Act
      const promise = service.getCertificatePreview(user.id, course.id);

      // Assert
      await expect(promise).rejects.toThrow(UnauthorizedException);
    });
    it("should throw NotFound when course does not exist", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(null);

      // Act
      const promise = service.getCertificatePreview(user.id, course.id);

      // Assert
      await expect(promise).rejects.toThrow(NotFoundException);
    });
    it("should throw NotFound when course does not have certificates", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: false,
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);

      // Act
      const promise = service.getCertificatePreview(user.id, course.id);

      // Assert
      await expect(promise).rejects.toThrow(NotFoundException);
    });
    it("should return certificate preview", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);

      // Act
      const promise = service.getCertificatePreview(user.id, course.id);

      // Assert
      await expect(promise).resolves.toStrictEqual({
        price: 1000,
        course: {
          title: course.title,
          author: {
            firstName: course.author.firstName,
            lastName: course.author.lastName,
          },
        },
      });
    });
  });

  describe("createOrder", () => {
    it("should throw Unauthorized when user does not exist", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };
      const orderRequest: OrderRequest = {
        address: "Green Hill Zone",
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(null);

      // Act
      const promise = service.createOrder(user.id, course.id, orderRequest);

      // Assert
      expect(promise).rejects.toThrow(UnauthorizedException);
    });
    it("should throw NotFound when course does not exist", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };
      const orderRequest: OrderRequest = {
        address: "Green Hill Zone",
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(null);

      // Act
      const promise = service.createOrder(user.id, course.id, orderRequest);

      // Assert
      expect(promise).rejects.toThrow(NotFoundException);
    });
    it("should throw NotFound when course does not have certificates", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: false,
      };
      const orderRequest: OrderRequest = {
        address: "Green Hill Zone",
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);

      // Act
      const promise = service.createOrder(user.id, course.id, orderRequest);

      // Assert
      expect(promise).rejects.toThrow(NotFoundException);
    });
    it("should throw Forbidden when user cannot order certificates", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };
      const orderRequest: OrderRequest = {
        address: "Green Hill Zone",
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);
      jest
        .spyOn(userCourseInfoService, "checkIfUserCanBuyCertificate")
        .mockResolvedValueOnce(false);

      // Act
      const promise = service.createOrder(user.id, course.id, orderRequest);

      // Assert
      expect(promise).rejects.toThrow(ForbiddenException);
    });
    test.each([null, "", "    ", "\t", "\n", "a".repeat(301)])(
      "should throw BadRequest when address invalid",
      async (address: string) => {
        // Arrange
        const user: User = {
          id: "15119b44-02dd-4bd2-b16a-676909c2b505",
          firstName: "Pie",
          lastName: "LastPie",
          avatar: null,
          gender: 0,
        };
        const author: User = {
          id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
          firstName: "Princess",
          lastName: "Peach",
          avatar: null,
          gender: 0,
        };
        const course: Course = {
          id: 1,
          title: "How to escape from turtle-like dragons",
          author: author,
          hasCertificates: true,
        };
        const orderRequest: OrderRequest = {
          address: address,
        };

        jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
        jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);
        jest
          .spyOn(userCourseInfoService, "checkIfUserCanBuyCertificate")
          .mockResolvedValueOnce(true);

        // Act
        const promise = service.createOrder(user.id, course.id, orderRequest);

        // Assert
        expect(promise).rejects.toThrow(BadRequestException);
      }
    );
    it("should throw BadRequest when external reports that address invalid", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };
      const orderRequest: OrderRequest = {
        address: "Green Hill Zone",
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);
      jest
        .spyOn(userCourseInfoService, "checkIfUserCanBuyCertificate")
        .mockResolvedValueOnce(true);
      const response: AxiosResponse<any, any> = {
        data: {
          features: [],
        },
        status: 200,
        statusText: "",
        headers: undefined,
        config: undefined,
      };
      jest.spyOn(httpService, "get").mockReturnValueOnce(
        new Observable<AxiosResponse<any, any>>((subscriber) => {
          subscriber.next(response);
          subscriber.complete();
        })
      );

      // Act
      const promise = service.createOrder(user.id, course.id, orderRequest);

      // Assert
      expect(promise).rejects.toThrow(BadRequestException);
    });
    it("should throw BadRequest when address valid but external returns different address", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };
      const orderRequest: OrderRequest = {
        address: "Green Hill Zone",
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);
      jest
        .spyOn(userCourseInfoService, "checkIfUserCanBuyCertificate")
        .mockResolvedValueOnce(true);
      const response: AxiosResponse<any, any> = {
        data: {
          features: [
            {
              properties: {
                full_address: "Not this one",
                feature_type: "address",
              },
            },
          ],
        },
        status: 200,
        statusText: "",
        headers: undefined,
        config: undefined,
      };
      jest.spyOn(httpService, "get").mockReturnValueOnce(
        new Observable<AxiosResponse<any, any>>((subscriber) => {
          subscriber.next(response);
          subscriber.complete();
        })
      );

      // Act
      const promise = service.createOrder(user.id, course.id, orderRequest);

      // Assert
      expect(promise).rejects.toThrow(BadRequestException);
    });
    it("should create and return new order", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };
      const orderRequest: OrderRequest = {
        address: "Green Hill Zone",
      };

      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest.spyOn(courseService, "findOne").mockResolvedValueOnce(course);
      jest
        .spyOn(userCourseInfoService, "checkIfUserCanBuyCertificate")
        .mockResolvedValueOnce(true);
      const response: AxiosResponse<any, any> = {
        data: {
          features: [
            {
              properties: {
                full_address: orderRequest.address,
                feature_type: "address",
              },
            },
          ],
        },
        status: 200,
        statusText: "",
        headers: undefined,
        config: undefined,
      };
      jest.spyOn(httpService, "get").mockReturnValueOnce(
        new Observable<AxiosResponse<any, any>>((subscriber) => {
          subscriber.next(response);
          subscriber.complete();
        })
      );
      const orderIdResponse: OrderIdResponse = {
        orderId: "6b4d0e9e-d92e-45d9-a435-d3de5a776ed2",
      };
      const orderCreate = jest
        .spyOn(orderService, "create")
        .mockResolvedValueOnce(orderIdResponse.orderId);

      // Act
      const promise = service.createOrder(user.id, course.id, orderRequest);

      // Assert
      expect(promise).resolves.toStrictEqual(orderIdResponse);
      await promise;
      expect(orderCreate).toHaveBeenCalledWith(
        user,
        course,
        orderRequest.address
      );
    });
  });

  describe("getAvailableCertificates", () => {
    it("should throw Unauthorized when user does not exist", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      jest.spyOn(userService, "findOne").mockResolvedValueOnce(null);

      // Act
      const promise = service.getAvailableCertificates(user.id);

      // Assert
      expect(promise).rejects.toThrow(UnauthorizedException);
    });
    it("should return available certificates", async () => {
      // Arrange
      const user: User = {
        id: "15119b44-02dd-4bd2-b16a-676909c2b505",
        firstName: "Pie",
        lastName: "LastPie",
        avatar: null,
        gender: 0,
      };
      const author: User = {
        id: "2d267225-5cb6-41c9-8add-6ed3a8234f45",
        firstName: "Princess",
        lastName: "Peach",
        avatar: null,
        gender: 0,
      };
      const course: Course = {
        id: 1,
        title: "How to escape from turtle-like dragons",
        author: author,
        hasCertificates: true,
      };
      const list: Course[] = [course];
      jest.spyOn(userService, "findOne").mockResolvedValueOnce(user);
      jest
        .spyOn(userCourseInfoService, "getAllAvailableCertificates")
        .mockResolvedValueOnce(list);
      jest
        .spyOn(orderService, "existsOrderWithCourseUser")
        .mockResolvedValueOnce(false);

      // Act
      const promise = service.getAvailableCertificates(user.id);

      // Assert
      expect(promise).resolves.toStrictEqual([
        { courseTitle: course.title, courseId: course.id },
      ]);
    });
  });
});
