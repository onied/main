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
import { UserCourseInfoService } from "../user-course-info/user-course-info.service";
import { UserCourseInfo } from "../user-course-info/user-course-info.entity";
import { NotFoundException } from "@nestjs/common/exceptions";
import { CertificatePreview } from "./dto/response/certificatePreview";
import { CertificateListCard } from "./dto/response/certificateListCard";
import { OrderRequest } from "./dto/request/orderRequest";
import { OrderIdResponse } from "./dto/response/orderIdResponse";

describe("CertificateController", () => {
  let controller: CertificateController;
  let certificateService: CertificateService;

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
        CertificateService,
      ],
    }).compile();

    controller = module.get<CertificateController>(CertificateController);
    certificateService = module.get<CertificateService>(CertificateService);
  });

  it("should be defined", () => {
    expect(controller).toBeDefined();
  });

  describe("getCertificatePreview", () => {
    it("should throw NotFound when courseId is not a number", async () => {
      // Arrange

      // Act
      const promise = controller.getCertificatePreview(
        "a31c13c6-09a3-44e1-a6dd-98b747f992f6",
        "asjldkfjskdfj"
      );

      // Assert
      await expect(promise).rejects.toThrow(NotFoundException);
    });
    it("should pass options to service correctly", async () => {
      // Arrange
      const preview: CertificatePreview = {
        price: 0,
        course: {
          title: "title",
          author: {
            firstName: "firstName",
            lastName: "lastName",
          },
        },
      };
      const getCertificatePreview = jest
        .spyOn(certificateService, "getCertificatePreview")
        .mockResolvedValueOnce(Promise.resolve(preview));
      const userId = "a31c13c6-09a3-44e1-a6dd-98b747f992f6";
      const courseId = 1;

      // Act
      const promise = controller.getCertificatePreview(
        userId,
        courseId.toString()
      );

      // Assert
      await expect(promise).resolves.toStrictEqual(preview);
      expect(getCertificatePreview).toHaveBeenCalledWith(userId, courseId);
    });
  });

  describe("getCertificatesList", () => {
    it("should pass options to service correctly", async () => {
      // Arrange
      const response: CertificateListCard[] = [
        {
          courseTitle: "courseTitle",
          courseId: 1,
        },
      ];
      const getAvailableCertificates = jest
        .spyOn(certificateService, "getAvailableCertificates")
        .mockResolvedValueOnce(Promise.resolve(response));
      const userId = "a31c13c6-09a3-44e1-a6dd-98b747f992f6";

      // Act
      const promise = controller.getCertificatesList(userId);

      // Assert
      await expect(promise).resolves.toStrictEqual(response);
      expect(getAvailableCertificates).toHaveBeenCalledWith(userId);
    });
  });

  describe("createOrder", () => {
    it("should throw NotFound when courseId is not a number", async () => {
      // Arrange
      const orderRequest: OrderRequest = {
        address: "address",
      };

      // Act
      const promise = async () =>
        await controller.createOrder(
          "a31c13c6-09a3-44e1-a6dd-98b747f992f6",
          "asjldkfjskdfj",
          orderRequest
        );

      // Assert
      await expect(promise).rejects.toThrow(NotFoundException);
    });
    it("should pass options to service correctly", async () => {
      // Arrange
      const orderRequest: OrderRequest = {
        address: "address",
      };
      const response: OrderIdResponse = {
        orderId: "5595e87b-f3a2-410a-8815-c6ec27e933a7",
      };
      const createOrder = jest
        .spyOn(certificateService, "createOrder")
        .mockResolvedValueOnce(Promise.resolve(response));
      const userId = "a31c13c6-09a3-44e1-a6dd-98b747f992f6";
      const courseId = 1;

      // Act
      const promise = controller.createOrder(
        userId,
        courseId.toString(),
        orderRequest
      );

      // Assert
      await expect(promise).resolves.toStrictEqual(response);
      expect(createOrder).toHaveBeenCalledWith(userId, courseId, orderRequest);
    });
  });
});
